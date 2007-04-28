/*
BUGx: An Asp.Net Bug Tracking tool.
Copyright (C) 2007 Olivier Bossaer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Wavenet, hereby disclaims all copyright interest in
the library `BUGx' (An Asp.Net Bug Tracking tool) written
by Olivier Bossaer. (olivier.bossaer@gmail.com)
*/
using System;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Bugx.ReBug
{
    class PropertyGridInspector : ICustomTypeDescriptor
    {
        class PropertyGridTypeConverter : TypeConverter
        {
            PropertyGridTypeConverter()
            {
            }
            public static readonly PropertyGridTypeConverter Instance = new PropertyGridTypeConverter();
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(value, attributes);
            }
            public override bool GetPropertiesSupported(ITypeDescriptorContext context)
            {
                if (context != null && context.PropertyDescriptor != null)
                {
                    PropertyGridInspector data = context.PropertyDescriptor.GetValue(context.Instance) as PropertyGridInspector;
                    if (data == null)
                    {
                        return false;
                    }
                    IDictionary dictionary = data._Data as IDictionary;
                    if (dictionary != null && dictionary.Count == 0)
                    {
                        return false;
                    }
                    NameValueCollection collection = data._Data as NameValueCollection;
                    if (collection != null && collection.Count == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    PropertyGridInspector data = value as PropertyGridInspector;
                    if (data != null)
                    {
                        Type type = data._Data.GetType();
                        if (type == typeof(string) || type == typeof(Uri))
                        {
                            return Convert.ToString(data._Data, CultureInfo.InvariantCulture);
                        }
                        DebuggerDisplayAttribute[] attributes = (DebuggerDisplayAttribute[])type.GetCustomAttributes(typeof(DebuggerDisplayAttribute), true);
                        if (attributes.Length == 1)
                        {
                            DebuggerDisplayAttribute text = attributes[0];
                            return Regex.Replace(text.Value, @"\{([^\}]+)\}", delegate(Match match)
                            {
                                try
                                {
                                    return Convert.ToString(type.GetProperty(match.Groups[1].Value).GetValue(data._Data, null));
                                }
                                catch (Exception exception)
                                {
                                    return "[" + exception.Message + "]";
                                }
                            });
                        }
                        return data._Data.GetType().FullName;
                    }
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        class PropertyGridPropertyDescriptor : PropertyDescriptor
        {
            PropertyDescriptor _InnerDescriptor;
            public override TypeConverter Converter
            {
                get
                {
                    return PropertyGridTypeConverter.Instance;
                }
            }
            public PropertyGridPropertyDescriptor(PropertyDescriptor innerDescriptor)
                : base(innerDescriptor.Name, null)
            {
                _InnerDescriptor = innerDescriptor;
            }

            public override bool CanResetValue(object component)
            {
                return _InnerDescriptor.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get
                {
                    return _InnerDescriptor.ComponentType;
                }
            }

            public override object GetValue(object component)
            {
                object result = _InnerDescriptor.GetValue(component);
                if (result != null)
                {
                    return new PropertyGridInspector(result);
                }
                return result;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return _InnerDescriptor.IsReadOnly;
                }
            }

            public override Type PropertyType
            {//This value is return to avoid any editor for complex objects
                get
                {
                    return typeof(string);
                }
            }

            public override void ResetValue(object component)
            {
                _InnerDescriptor.ResetValue(component);
            }

            public override void SetValue(object component, object value)
            {
                _InnerDescriptor.SetValue(component, value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return _InnerDescriptor.ShouldSerializeValue(component);
            }

            public override string Category
            {
                get
                {
                    return _InnerDescriptor.Category;
                }
            }

            public override string Description
            {
                get
                {
                    return _InnerDescriptor.Description;
                }
            }

        }
        class NameValueDescriptor : PropertyDescriptor
        {
            NameValueCollection _NameValue;
            string _Key;
            public NameValueDescriptor(NameValueCollection nameValue, string key)
                : base(key, null)
            {
                _NameValue = nameValue;
                _Key = key;
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return null;
                }
            }

            public override object GetValue(object component)
            {
                return _NameValue[_Key];
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return typeof(string);
                }
            }

            public override void ResetValue(object component)
            {

            }

            public override void SetValue(object component, object value)
            {
                _NameValue[_Key] = Convert.ToString(value);
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
        class NameObjectDescriptor : PropertyDescriptor
        {
            IDictionary _NameValue;
            object _Key;
            public NameObjectDescriptor(IDictionary nameValue, object key)
                : base(Convert.ToString(key), null)
            {
                _NameValue = nameValue;
                _Key = key;
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return null;
                }
            }

            public override TypeConverter Converter
            {
                get
                {
                    object value = _NameValue[_Key];
                    if (value != null && value.GetType().IsValueType)
                    {
                        return null;
                    }
                    return PropertyGridTypeConverter.Instance;
                }
            }

            public override object GetValue(object component)
            {
                object result = _NameValue[_Key];
                if (result != null && !result.GetType().IsValueType)
                {
                    return new PropertyGridInspector(result);
                }
                return result;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return _NameValue.IsReadOnly;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    return GetValue(null).GetType();
                }
            }

            public override void ResetValue(object component)
            {

            }

            public override void SetValue(object component, object value)
            {
                _NameValue[_Key] = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
        class ListDescriptor : PropertyDescriptor
        {
            IList _List;
            int _Index;
            public ListDescriptor(IList list, int index)
                : base("[" + index + "]", null)
            {
                _List = list;
                _Index = index;
            }
            public override bool CanResetValue(object component)
            {
                return false;
            }

            public override Type ComponentType
            {
                get
                {
                    return null;
                }
            }

            public override TypeConverter Converter
            {
                get
                {
                    object value = _List[_Index];
                    if (value != null && value.GetType().IsValueType)
                    {
                        return null;
                    }
                    return PropertyGridTypeConverter.Instance;
                }
            }

            public override object GetValue(object component)
            {
                object result = _List[_Index];
                if (result != null && !result.GetType().IsValueType)
                {
                    return new PropertyGridInspector(result);
                }
                return result;
            }

            public override bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public override Type PropertyType
            {
                get
                {
                    object type = GetValue(null);
                    return type != null ? type.GetType() : null;
                }
            }

            public override void ResetValue(object component)
            {

            }

            public override void SetValue(object component, object value)
            {
                _List[_Index] = value;
            }

            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }
        object _Data;
        public PropertyGridInspector(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            _Data = data;
        }
        #region ICustomTypeDescriptor Members

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(_Data);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(_Data);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(_Data);
        }

        public TypeConverter GetConverter()
        {
            TypeConverter result = TypeDescriptor.GetConverter(_Data);
            if (result == null || result.GetType() == typeof(TypeConverter))
            {
                return PropertyGridTypeConverter.Instance;
            }
            return result;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(_Data);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(_Data);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(_Data, editorBaseType);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(_Data, attributes);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(_Data);
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }
        public PropertyDescriptorCollection GetProperties()
        {
            ReBugContext context = _Data as ReBugContext;
            if (context != null)
            {
                return GetPropertiesForContext(context);
            }
            NameValueCollection nameValue = _Data as NameValueCollection;
            if (nameValue != null)
            {
                return GetVirtualPropertiesForNameValue(nameValue);
            }
            IDictionary nameObject = _Data as IDictionary;
            if (nameObject != null)
            {
                return GetVirtualPropertiesForNameObject(nameObject);
            }
            IList list = _Data as IList;
            if (list != null)
            {
                return GetVirtualPropertiesForList(list);
            }
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(_Data))
            {
                if (descriptor.PropertyType.IsValueType || descriptor.PropertyType == typeof(string))
                {
                    result.Add(descriptor);
                }
                else
                {
                    result.Add(new PropertyGridPropertyDescriptor(descriptor));
                }
            }
            return result;
        }

        static PropertyDescriptorCollection GetVirtualPropertiesForList(IList list)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            for (int i = 0; i < list.Count; i++)
            {
                result.Add(new ListDescriptor(list, i));
            }
            return result;
        }

        static PropertyDescriptorCollection GetPropertiesForContext(ReBugContext context)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(context))
            {
                if (descriptor.PropertyType.IsValueType || descriptor.PropertyType == typeof(string))
                {
                    result.Add(descriptor);
                }
                else
                {
                    object data = descriptor.GetValue(context);
                    if (data == null)
                    {
                        continue;
                    }
                    IDictionary dictionary = data as IDictionary;
                    if (dictionary != null && dictionary.Count == 0)
                    {
                        continue;
                    }
                    ICollection collection = data as ICollection;
                    if (collection != null && collection.Count == 0)
                    {
                        continue;
                    }
                    result.Add(new PropertyGridPropertyDescriptor(descriptor));
                }
            }
            return result;
        }

        static PropertyDescriptorCollection GetVirtualPropertiesForNameObject(IDictionary nameObject)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            foreach (object key in nameObject.Keys)
            {
                result.Add(new NameObjectDescriptor(nameObject, key));
            }
            return result;
        }

        static PropertyDescriptorCollection GetVirtualPropertiesForNameValue(NameValueCollection nameValue)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            foreach (string key in nameValue.Keys)
            {
                result.Add(new NameValueDescriptor(nameValue, key));
            }
            return result;
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _Data;
        }
        #endregion

    }
}
