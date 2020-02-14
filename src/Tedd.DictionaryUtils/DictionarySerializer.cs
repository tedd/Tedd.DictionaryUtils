﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Tedd
{
    public class DictionarySerializerConfig
    {
        public List<Type> DoNotFlattenTypes = new List<Type>();

    }
    public class DictionarySerializer
    {
        private readonly DictionarySerializerConfig _config;

        private readonly static HashSet<Type> _knownTypes = new HashSet<Type>()
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/types#simple-types
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(char),
            typeof(float),
            typeof(double),
            typeof(bool),
            typeof(decimal),
            // Other
            typeof(string),
            typeof(DateTime),
            typeof(IPAddress),
            typeof(IDictionary),
            typeof(IDictionary<,>),
        };

        private readonly HashSet<Type> _doNotFlattenTypes;

        public DictionarySerializer()
        {
            _config = new DictionarySerializerConfig();
        }

        public DictionarySerializer(DictionarySerializerConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _config = config;
            if (_config.DoNotFlattenTypes != null)
                _doNotFlattenTypes = _config.DoNotFlattenTypes.ToHashSet(s => s);
        }

        public Dictionary<string, object> ToFlatDictionary<T>(T @object, FlattenMemberType memberType = FlattenMemberType.Property)
        {
            var dic = new Dictionary<string, object>();
            ToFlatDictionaryInt<T>(dic, "", @object, memberType);
            return dic;
        }

        private void ToFlatDictionaryInt<T>(Dictionary<string, object> dic, string prefix, T @object, FlattenMemberType memberType)
        {

            // Get all public 
            var type = @object.GetType();
            if ((memberType & FlattenMemberType.Property) != 0)
                FlattenProperies(dic, prefix, @object, memberType, type);

        }

        private void FlattenProperies<T>(Dictionary<string, object> dic, string prefix, T @object, FlattenMemberType memberType, Type type)
        {
            if (_knownTypes.Contains(type) || (_doNotFlattenTypes != null && _doNotFlattenTypes.Contains(type)))
            {
                // Property directly
                dic.Add(prefix, @object);
                return;
            }

            prefix = string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ".";


            // TODO: Properties can be cached
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var pt = property.PropertyType;
                var key = prefix + property.Name;

                var ptil = pt.GetInterfaces();

                if (_knownTypes.Contains(pt) || _knownTypes.Contains(ptil) // Quick lookup for known types
                    || (_doNotFlattenTypes != null && (_doNotFlattenTypes.Contains(pt) && _doNotFlattenTypes.Contains(ptil)))
                    || property.GetCustomAttributes(typeof(DoNotFlattenAttribute), true)?.Length > 0 // Or with DoNotFlattenAttribute
                      //|| property.PropertyType.GetGenericTypeDefinition()
                )
                {
                    // Property directly
                    var dval = property.GetValue(@object);
                    dic.Add(key, dval);
                    continue;
                }

    
                if (pt.IsArray)
                {
                    var list = (object[])property.GetValue(@object);
                    for (var c = 0; c < list.Length; c++)
                        ToFlatDictionaryInt(dic, $"{key}[{c}]", list[c], memberType);
                    continue;
                }
                if (ptil.Contains(typeof(IList)))
                {
                    var list = (IList)property.GetValue(@object);
                    for (var c = 0; c < list.Count; c++)
                        ToFlatDictionaryInt(dic, $"{key}[{c}]", list[c], memberType);
                    continue;
                }
                if (ptil.Contains(typeof(IEnumerable)))
                {
                    var c = 0;
                    foreach (var e in (IEnumerable)property.GetValue(@object))
                    {
                        ToFlatDictionaryInt(dic, $"{key}[{c}]", e, memberType);
                        c++;
                    }
                    continue;
                }
               

                var val = property.GetValue(@object);


                // Go into property
                ToFlatDictionaryInt(dic, key, val, memberType);
            }
        }
    }
}