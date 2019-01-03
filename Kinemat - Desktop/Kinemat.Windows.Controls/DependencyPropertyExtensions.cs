using System;
using System.Windows;

namespace Kinemat.Windows
{
    /// <summary>
    /// Contains utility methods for registering dependency properties with extended metadata.
    /// 
    /// </summary>
    public static class DependencyPropertyExtensions
    {
        /// <summary>
        /// Registers a dependency property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType)
        {
            return DependencyProperty.Register(name, propertyType, ownerType, (PropertyMetadata)null);
        }

        /// <summary>
        /// Registers a dependency property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return DependencyProperty.Register(name, propertyType, ownerType, typeMetadata);
        }

        /// <summary>
        /// Registers a dependency property with the specified property name, property type, owner type, property metadata and validate value callback.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param><param name="validateValueCallback">Callback that validates the new value for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.Register(name, propertyType, ownerType, typeMetadata, validateValueCallback);
        }

        /// <summary>
        /// Registers an attached property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType)
        {
            return DependencyProperty.RegisterAttached(name, propertyType, ownerType, (PropertyMetadata)null);
        }

        /// <summary>
        /// Registers an attached property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return DependencyProperty.RegisterAttached(name, propertyType, ownerType, typeMetadata);
        }

        /// <summary>
        /// Registers an attached property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param><param name="validateValueCallback">Callback that validates the new value for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyProperty RegisterAttached(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.RegisterAttached(name, propertyType, ownerType, typeMetadata, validateValueCallback);
        }

        /// <summary>
        /// Registers a read-only attached property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return DependencyProperty.RegisterAttachedReadOnly(name, propertyType, ownerType, typeMetadata);
        }

        /// <summary>
        /// Registers a read-only attached property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param><param name="validateValueCallback">Callback that validates the new value for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyPropertyKey RegisterAttachedReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.RegisterAttachedReadOnly(name, propertyType, ownerType, typeMetadata, validateValueCallback);
        }

        /// <summary>
        /// Registers a read-only dependency property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return DependencyProperty.RegisterReadOnly(name, propertyType, ownerType, typeMetadata);
        }

        /// <summary>
        /// Registers a read-only dependency property with the specified property name, property type, owner type, and property metadata.
        /// 
        /// </summary>
        /// <param name="name">The name of the dependency property to register.
        ///             </param><param name="propertyType">The type of the property.
        ///             </param><param name="ownerType">The owner type that is registering the dependency property.
        ///             </param><param name="typeMetadata">Property metadata for the dependency property.
        ///             </param><param name="validateValueCallback">Callback that validates the new value for the dependency property.
        ///             </param>
        /// <returns>
        /// A dependency property identifier that should be used to set the value of a public static readonly field in your class. That identifier is then used to reference the dependency property later, for operations such as setting its value programmatically or obtaining metadata.
        /// 
        /// </returns>
        public static DependencyPropertyKey RegisterReadOnly(string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata, ValidateValueCallback validateValueCallback)
        {
            return DependencyProperty.RegisterReadOnly(name, propertyType, ownerType, typeMetadata, validateValueCallback);
        }
    }
}
