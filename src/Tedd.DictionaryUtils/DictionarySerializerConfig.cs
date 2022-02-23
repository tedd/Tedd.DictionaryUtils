#if SERIALIZER
using System;
using System.Collections.Generic;

namespace Tedd;

public class DictionarySerializerConfig
{
    public List<Type> DoNotFlattenTypes = new List<Type>();

}
#endif