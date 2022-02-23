#if DESERIALIZER || SERIALIZER

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tedd;

    internal static class TypeCache
    {
        private struct TypeBinding : IEquatable<TypeBinding>
        {
            public Type Type;
            public BindingFlags BindingFlags;

            #region Equality members

            /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
            /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(TypeBinding other)
            {
                return Equals(Type, other.Type) && BindingFlags == other.BindingFlags;
            }

            /// <summary>Indicates whether this instance and a specified object are equal.</summary>
            /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false. </returns>
            /// <param name="obj">The object to compare with the current instance. </param>
            public override bool Equals(object obj)
            {
                return obj is TypeBinding other && Equals(other);
            }

            /// <summary>Returns the hash code for this instance.</summary>
            /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (int)BindingFlags;
                }
            }

            #endregion
        }

        private static Dictionary<TypeBinding, PropertyInfo[]> TypeToPropertyInfo = new Dictionary<TypeBinding, PropertyInfo[]>();
        private static Dictionary<PropertyInfo, bool> DoNotFlattenAttribute = new Dictionary<PropertyInfo, bool>();
        private static Dictionary<Type, Type[]> TypeInterfaces = new Dictionary<Type, Type[]>();

        public static PropertyInfo[] GetProperties(Type type, BindingFlags bindingFlags)
        {
            lock (TypeToPropertyInfo)
                return TypeToPropertyInfo.GetOrAdd(
                    new TypeBinding() { Type = type, BindingFlags = bindingFlags },
                    () => type.GetProperties(bindingFlags));
        }

        public static bool GetDoNotFlattenAttribute(PropertyInfo property)
        {
            lock (DoNotFlattenAttribute)
                return DoNotFlattenAttribute.GetOrAdd(property,
                    () => property.GetCustomAttributes(typeof(DoNotFlattenAttribute), true)?.Length > 0);

        }

        public static Type[] GetInterfaces(Type pt)
        {
            lock (TypeInterfaces)
                return TypeInterfaces.GetOrAdd(pt, () => pt.GetInterfaces());
        }
    }
#endif