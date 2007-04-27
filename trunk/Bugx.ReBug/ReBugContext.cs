using System;
using System.Collections.Generic;
using System.Text;
using Bugx.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Xml;
using System.Runtime.Serialization;
using System.Drawing.Design;

namespace Bugx.ReBug
{
    [Serializable]
    public class ReBugContext
    {
        Uri _Url;
        HttpValueCollection _Form;
        HttpValueCollection _QueryString;
        HttpValueCollection _Cookies;
        HttpValueCollection _Headers;
        Dictionary<string, object> _Session = new Dictionary<string, object>();
        Dictionary<string, object> _Cache = new Dictionary<string, object>();
        Dictionary<object, object> _Context = new Dictionary<object, object>();
        Exception _Exception;

        public ReBugContext(XmlDocument bug)
        {
            _QueryString = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/queryString"));

            _Form = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/form"));

            XmlNode cookie = bug.SelectSingleNode("/bugx/headers/Cookie");
            if (cookie != null)
            {
                _Cookies = HttpValueCollection.CreateCollectionFromCookieHeader(cookie.InnerText);
            }
            _Headers = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/headers"));

            // bug.SelectSingleNode("/bugx/sessionVariables");

            //bug.SelectSingleNode("/bugx/chacheVariables");


            XmlNode exception = bug.SelectSingleNode("/bugx/exception");
            if (exception != null)
            {
                _Exception = (Exception)BugSerializer.Deserialize(exception.InnerText);
                /*ExceptionExplorer.SelectedObject = new ExceptionDescriptor((Exception)BugSerializer.Deserialize(exception.InnerText));
                VariableTree.Nodes.Add("Exception", "Exception", "Variable", "Variable").Tag = new ObjectInspector("Exception", ExceptionExplorer.SelectedObject);*/
            }
        }

        [Category("Request")]
        [Description("Gets information about the URL of the current request.")]
        public Uri Url
        {
            get { return this._Url; }
            set { this._Url = value; }
        }

        [Category("Request")]
        [Description("Gets a collection of form variables.")]
        public HttpValueCollection Form
        {
            get { return this._Form; }
        }

        [Category("Request")]
        [Description("Gets the collection of HTTP query string variables.")]
        public HttpValueCollection QueryString
        {
            get { return this._QueryString; }
        }

        [Category("Request")]
        [Description("Gets a collection of cookies sent by the client.")]
        public HttpValueCollection Cookies
        {
            get { return this._Cookies; }
        }

        [Category("Request")]
        [Description("Gets a collection of HTTP headers.")]
        public HttpValueCollection Headers
        {
            get { return this._Headers; }
        }

        [Category("Environement")]
        [Description("Gets the System.Web.SessionState.HttpSessionState object for the current HTTP request.")]
        public Dictionary<string, object> Session
        {
            get { return this._Session; }
        }

        [Category("Environement")]
        [Description("Gets the System.Web.Caching.Cache object for the current HTTP request.")]
        public Dictionary<string, object> Cache
        {
            get { return this._Cache; }
        }

        [Category("Environement")]
        [Description("Gets a key/value collection that can be used to organize and share data between an System.Web.IHttpModule interface and an System.Web.IHttpHandler interface during an HTTP request.")]
        public Dictionary<object, object> Context
        {
            get { return this._Context; }
        }
        
        [Category("User - Exception")]
        [Description("Gets the first error accumulated during HTTP request processing.")]
        public Exception Exception
        {
            get { return this._Exception; }
            set { this._Exception = value; }
        }
    }

    class PropertyGridInspector : ICustomTypeDescriptor
    {
        class PropertyGridTypeConverter : TypeConverter
        {
            PropertyGridTypeConverter() { }
            public static readonly PropertyGridTypeConverter Instance = new PropertyGridTypeConverter();
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                return TypeDescriptor.GetProperties(value, attributes); ;
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
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
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
                get { return PropertyGridTypeConverter.Instance; }
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
                get { return _InnerDescriptor.ComponentType; }
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
                get { return _InnerDescriptor.IsReadOnly; }
            }

            public override Type PropertyType
            {//This value is return to avoid any editor for complex objects
                get { return typeof(string); }
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
                get { return _InnerDescriptor.Category; }
            }

            public override string Description
            {
                get { return _InnerDescriptor.Description; }
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
                get { return null; }
            }

            public override object GetValue(object component)
            {
                return _NameValue[_Key];
            }

            public override bool IsReadOnly
            {
                get { return false; }
            }

            public override Type PropertyType
            {
                get { return typeof(string); }
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
                get { return null; }
            }

            public override object GetValue(object component)
            {
                return _NameValue[_Key];
            }

            public override bool IsReadOnly
            {
                get { return _NameValue.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return typeof(string); }
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
