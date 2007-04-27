using System;
using System.Collections.Generic;
using System.Web.Caching;
using Bugx.Web;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.Reflection;

namespace Bugx.ReBug
{
    [Serializable]
    public class ReBugContext
    {
        #region Fields
        Uri _Url;
        HttpValueCollection _Form;
        HttpValueCollection _QueryString;
        HttpValueCollection _Cookies;
        HttpValueCollection _Headers;
        Dictionary<string, object> _Session;
        Dictionary<string, object> _Cache;
        Dictionary<string, object> _Application;
        Dictionary<object, object> _Context;
        string _PathInfo;
        Exception _Exception; 
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ReBugContext"/> class.
        /// </summary>
        /// <param name="bug">The bug.</param>
        protected ReBugContext(XmlDocument bug)
        {
            _QueryString = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/queryString"));

            _Form = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/form"));

            XmlNode cookie = bug.SelectSingleNode("/bugx/headers/Cookie");
            if (cookie != null)
            {
                _Cookies = HttpValueCollection.CreateCollectionFromCookieHeader(cookie.InnerText);
            }
            _Headers = HttpValueCollection.CreateCollectionFromXmlNode(bug.SelectSingleNode("/bugx/headers"));

            _Session = FillNameValue(bug.SelectNodes("/bugx/sessionVariables/add"));
            _Cache = FillNameValue(bug.SelectNodes("/bugx/cacheVariables/add"));
            _Application = FillNameValue(bug.SelectNodes("/bugx/applicationVariables/add"));
            _Context = FillHashtable(bug.SelectNodes("/bugx/contextVariables/add"));

            XmlNode exception = bug.SelectSingleNode("/bugx/exception");
            if (exception != null)
            {
                _Exception = (Exception)BugSerializer.Deserialize(exception.InnerText);
            }
            XmlNode url = bug.SelectSingleNode("/bugx/url");
            if (url != null)
            {
                _Url = new Uri(url.InnerText);
            }
            XmlNode pathInfo = bug.SelectSingleNode("/bugx/pathInfo");
            if (pathInfo != null)
            {
                _PathInfo = pathInfo.InnerText;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ReBugContext"/> class.
        /// </summary>
        /// <param name="bug">The bug.</param>
        /// <returns></returns>
        public static ReBugContext Create(XmlDocument bug)
        {
            Type custom = Type.GetType(ConfigurationManager.AppSettings["CustomReBugContext"] ?? typeof(ReBugContext).AssemblyQualifiedName );
            if (custom != null && typeof(ReBugContext).IsAssignableFrom(custom))
            {
                ConstructorInfo constructor = custom.GetConstructor(BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.NonPublic, null, new Type[] {typeof (XmlDocument)}, null);
                if (constructor != null)
                {
                    return (ReBugContext)constructor.Invoke(new object[] { bug });
                }
            }
            return new ReBugContext(bug);
        } 
        #endregion

        #region Helpers
        /// <summary>
        /// Fills the name value.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        static Dictionary<string, object> FillNameValue(XmlNodeList nodes)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (XmlNode node in nodes)
            {
                object data;
                if (node.Attributes["value"] != null)
                {
                    data = Convert.ChangeType(node.Attributes["value"].Value, Type.GetType(node.Attributes["type"].Value));
                }
                else
                {
                    data = BugSerializer.Deserialize(node.InnerText);
                }
                result[node.Attributes["name"].Value] = data;
            }
            return result;
        }

        /// <summary>
        /// Fills the hashtable.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <returns></returns>
        static Dictionary<object, object> FillHashtable(XmlNodeList nodes)
        {
            Dictionary<object, object> result = new Dictionary<object, object>();
            foreach (XmlNode node in nodes)
            {
                object data;
                if (node.Attributes["value"] != null)
                {
                    data = Convert.ChangeType(node.Attributes["value"].Value, Type.GetType(node.Attributes["type"].Value));
                }
                else
                {
                    data = BugSerializer.Deserialize(node.InnerText);
                }
                if (node.Attributes["nameType"] != null)
                {
                    result[Convert.ChangeType(node.Attributes["name"].Value, Type.GetType(node.Attributes["nameType"].Value))] = data;
                }
                else
                {
                    result[BugSerializer.Deserialize(node.SelectSingleNode("key").InnerText)] = data;
                }
            }
            return result;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets information about the URL of the current request.
        /// </summary>
        /// <value>The URL.</value>
        [Category("Request")]
        [Description("Gets information about the URL of the current request.")]
        public Uri Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        /// <summary>
        /// Gets a collection of form variables.
        /// </summary>
        /// <value>The form.</value>
        [Category("Request")]
        [Description("Gets a collection of form variables.")]
        public HttpValueCollection Form
        {
            get
            {
                return _Form;
            }
        }

        /// <summary>
        /// Gets the collection of HTTP query string variables.
        /// </summary>
        [Category("Request")]
        [Description("Gets the collection of HTTP query string variables.")]
        public HttpValueCollection QueryString
        {
            get
            {
                return _QueryString;
            }
        }

        /// <summary>
        /// Gets a collection of cookies sent by the client.
        /// </summary>
        [Category("Request")]
        [Description("Gets a collection of cookies sent by the client.")]
        public HttpValueCollection Cookies
        {
            get
            {
                return _Cookies;
            }
        }

        /// <summary>
        /// Gets a collection of HTTP headers.
        /// </summary>
        [Category("Request")]
        [Description("Gets a collection of HTTP headers.")]
        public HttpValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.SessionState.HttpSessionState"/> object for the current HTTP request.
        /// </summary>
        [Category("Environement")]
        [Description("Gets the System.Web.SessionState.HttpSessionState object for the current HTTP request.")]
        public Dictionary<string, object> Session
        {
            get
            {
                return _Session;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.Caching.Cache"/> object for the current HTTP request.
        /// </summary>
        [Category("Environement")]
        [Description("Gets the System.Web.Caching.Cache object for the current HTTP request.")]
        public Dictionary<string, object> Cache
        {
            get
            {
                return _Cache;
            }
        }

        /// <summary>
        /// Gets a key/value collection that can be used to organize and share data between an <see cref="System.Web.IHttpModule"/> interface and an <see cref="System.Web.IHttpHandler"/> interface during an HTTP request.
        /// </summary>
        [Category("Environement")]
        [Description("Gets a key/value collection that can be used to organize and share data between an System.Web.IHttpModule interface and an System.Web.IHttpHandler interface during an HTTP request.")]
        public Dictionary<object, object> Context
        {
            get
            {
                return _Context;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Web.HttpApplicationState"/> object for the current HTTP request.
        /// </summary>
        [Category("Environement")]
        [Description("Gets the System.Web.HttpApplicationState object for the current HTTP request.")]
        public Dictionary<string, object> Application
        {
            get
            {
                return _Application;
            }
        }

        /// <summary>
        /// Gets the first error accumulated during HTTP request processing.
        /// </summary>
        [Category("User - Exception")]
        [Description("Gets the first error accumulated during HTTP request processing.")]
        public Exception Exception
        {
            get
            {
                return _Exception;
            }
            set
            {
                _Exception = value;
            }
        }

        /// <summary>
        /// Gets additional path information for a resource with a URL extension.
        /// </summary>
        [Category("Request")]
        [Description("Gets additional path information for a resource with a URL extension.")]
        public string PathInfo
        {
            get{ return _PathInfo; }
            set{ _PathInfo = value; }
        } 
        #endregion

        #region Methods
        /// <summary>
        /// Restores the environment.
        /// </summary>
        public virtual void RestoreEnvironment()
        {
            HttpContext context = HttpContext.Current;
            HttpSessionState session = context.Session;
            foreach (string key in Session.Keys)
            {
                session[key] = Session[key];
            }
            Cache cache = context.Cache;
            foreach (string key in Cache.Keys)
            {
                cache[key] = Cache[key];
            }
            IDictionary contextItems = context.Items;
            foreach (object key in Context.Keys)
            {
                contextItems[key] = Context[key];
            }
            HttpApplicationState application = context.Application;
            foreach (string key in application.Keys)
            {
                application[key] = Application[key];
            }
        } 
        #endregion
    }

    class PropertyGridInspector : ICustomTypeDescriptor
    {
        class PropertyGridTypeConverter : TypeConverter
        {
            PropertyGridTypeConverter() { }
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
                get { return _NameValue.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get{ return GetValue(null).GetType(); }
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
                get{ return null; }
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
                get{ return false; }
            }

            public override Type PropertyType
            {
                get
                {
                    object type = GetValue(null);
                    return type != null? type.GetType(): null;
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

        static  PropertyDescriptorCollection GetVirtualPropertiesForList(IList list)
        {
            PropertyDescriptorCollection result = new PropertyDescriptorCollection(null);
            for (int i = 0; i < list.Count; i++ )
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
