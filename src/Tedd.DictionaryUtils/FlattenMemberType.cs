#if DESERIALIZER || SERIALIZER
using System;

namespace Tedd;

[Flags]
public enum FlattenMemberType
{
    Property = 0b01,
    Field = 0b10,
    PropertyAndField = Property | Field
}
#endif