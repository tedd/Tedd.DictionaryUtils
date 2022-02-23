#if DESERIALIZER || SERIALIZER
using System;

namespace Tedd;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
public class DoNotFlattenAttribute : Attribute
{ }
#endif