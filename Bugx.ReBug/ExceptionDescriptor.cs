using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bugx.ReBug
{
    public class ExceptionDescriptor : ICustomTypeDescriptor
    {
        #region ExpandableInnerException
        /// <summary>
        /// This class is used to show properties of <c>InnerException</c>.
        /// </summary>
        class ExpandableInnerException : PropertyDescriptor
        {
            /// <summary>
            /// Base descriptor.
            /// </summary>
            PropertyDescriptor _BaseDescriptor;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExpandableInnerException"/> class.
            /// </summary>
            /// <param name="baseDescriptor">The base descriptor.</param>
            public ExpandableInnerException(PropertyDescriptor baseDescriptor)
                : base(baseDescriptor.Name, null)
            {
                _BaseDescriptor = baseDescriptor;
            }

            /// <summary>
            /// Returns ExpandableObjectConverter.
            /// </summary>
            public override TypeConverter Converter
            {
                get
                {
                    return new ExpandableObjectConverter();
                }
            }

            /// <summary>
            /// When overridden in a derived class, returns whether resetting an object changes its value.
            /// </summary>
            /// <param name="component">The component to test for reset capability.</param>
            /// <returns>
            /// true if resetting the component changes its value; otherwise, false.
            /// </returns>
            public override bool CanResetValue(object component)
            {
                return _BaseDescriptor.CanResetValue(component);
            }

            /// <summary>
            /// When overridden in a derived class, gets the type of the component this property is bound to.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.Type"></see> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"></see> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"></see> methods are invoked, the object specified might be an instance of this type.</returns>
            public override Type ComponentType
            {
                get
                {
                    return _BaseDescriptor.ComponentType;
                }
            }

            /// <summary>
            /// When overridden in a derived class, gets the current value of the property on a component.
            /// </summary>
            /// <param name="component">The component with the property for which to retrieve the value.</param>
            /// <returns>
            /// The value of a property for a given component.
            /// </returns>
            public override object GetValue(object component)
            {
                return new ExceptionDescriptor((Exception)_BaseDescriptor.GetValue(component));
            }

            /// <summary>
            /// When overridden in a derived class, gets a value indicating whether this property is read-only.
            /// </summary>
            /// <value></value>
            /// <returns>true if the property is read-only; otherwise, false.</returns>
            public override bool IsReadOnly
            {
                get
                {
                    return _BaseDescriptor.IsReadOnly;
                }
            }

            /// <summary>
            /// When overridden in a derived class, gets the type of the property.
            /// </summary>
            /// <value></value>
            /// <returns>A <see cref="T:System.Type"></see> that represents the type of the property.</returns>
            public override Type PropertyType
            {
                get
                {
                    return _BaseDescriptor.PropertyType;
                }
            }

            /// <summary>
            /// When overridden in a derived class, resets the value for this property of the component to the default value.
            /// </summary>
            /// <param name="component">The component with the property value that is to be reset to the default value.</param>
            public override void ResetValue(object component)
            {
                _BaseDescriptor.ResetValue(component);
            }

            /// <summary>
            /// When overridden in a derived class, sets the value of the component to a different value.
            /// </summary>
            /// <param name="component">The component with the property value that is to be set.</param>
            /// <param name="value">The new value.</param>
            public override void SetValue(object component, object value)
            {
                _BaseDescriptor.SetValue(component, value);
            }

            /// <summary>
            /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
            /// </summary>
            /// <param name="component">The component with the property to be examined for persistence.</param>
            /// <returns>
            /// true if the property should be persisted; otherwise, false.
            /// </returns>
            public override bool ShouldSerializeValue(object component)
            {
                return _BaseDescriptor.ShouldSerializeValue(component);
            }
        }
        #endregion

        /// <summary>
        /// Exception to display in <see cref="PropertyGrid"/>.
        /// </summary>
        Exception _Exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptor"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public ExceptionDescriptor(Exception exception)
        {
            _Exception = exception;
        }

        #region Default methods
        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The class name of the object, or null if the class does not have a name.
        /// </returns>
        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(_Exception, true);
        }

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.AttributeCollection"></see> containing the attributes for this object.
        /// </returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(_Exception, true);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The name of the object, or null if the object does not have a name.
        /// </returns>
        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(_Exception, true);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter"></see> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter"></see> for this object.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(_Exception, true);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptor"></see> that represents the default event for this object, or null if this object does not have events.
        /// </returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(_Exception, true);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that represents the default property for this object, or null if this object does not have properties.
        /// </returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(_Exception, true);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A <see cref="T:System.Type"></see> that represents the editor for this object.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> of the specified type that is the editor for this object, or null if the editor cannot be found.
        /// </returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(_Exception, editorBaseType, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"></see> that is used as a filter.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"></see> that represents the filtered events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(_Exception, attributes, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"></see> that represents the events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(_Exception, true);
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that represents the property whose owner is to be found.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the owner of the specified property.
        /// </returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _Exception;
        }

        #endregion

        #region Implementation
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection descriptors = TypeDescriptor.GetProperties(_Exception);
            List<PropertyDescriptor> result = new List<PropertyDescriptor>();
            for (int i = 0; i < descriptors.Count; i++ )
            {
                ICollection list = descriptors[i].GetValue(_Exception) as ICollection;
                string value = Convert.ToString(descriptors[i].GetValue(_Exception));
                if (string.IsNullOrEmpty(value) || (list != null && list.Count == 0))
                {
                    continue;
                }
                if (descriptors[i].Name == "InnerException")
                {
                    result.Add(new ExpandableInnerException(descriptors[i]));
                }
                else
                {
                    result.Add(descriptors[i]);
                }
            }
            PropertyDescriptor[] usedDescriptors = new PropertyDescriptor[result.Count];
            result.CopyTo(usedDescriptors);
            return new PropertyDescriptorCollection(usedDescriptors, true);
        }

        #endregion

        /// <summary>
        /// Returns type of current <c>exception</c>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return _Exception.GetType().FullName;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return _Exception.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            ExceptionDescriptor descriptor = obj as ExceptionDescriptor;
            if (descriptor == null)
            {
                return false;
            }
            return _Exception == descriptor._Exception;
        }

    }
}
