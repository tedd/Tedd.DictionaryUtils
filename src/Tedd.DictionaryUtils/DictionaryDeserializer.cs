#if DESERIALIZER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Tedd;

    public class DictionaryDeserializer
    {
        public T UnflattenDictionary<T>(Dictionary<string, object> dictionary)
        => UnflattenDictionary(dictionary, CreateDefault<T>());

        public T UnflattenDictionary<T>(Dictionary<string, object> dictionary, T objectToPopulate)
        {
            var obj = objectToPopulate;
            if (dictionary is null)
                throw new ArgumentNullException(nameof(dictionary));

            UnflattenInt(dictionary, "", obj);
            return obj;
        }

        private void UnflattenInt<T>(Dictionary<string, object> dic, string prefix, T @object)
        {
            prefix = string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ".";
            var memberType = @object.GetType();

            var properties = TypeCache.GetProperties(memberType, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (var property in properties)
            {
                var pt = property.PropertyType;
                var key = prefix + property.Name;

                Debug.WriteLine($"Processing: {key}");

                var ptil = TypeCache.GetInterfaces(pt);

                if (dic.TryGetValue(key, out var existingValue))
                {
                    // TODO: What do we do if value is null?

                    // We got a match, set it and we are done!
                    property.SetValue(@object, existingValue);
                    Debug.WriteLine($"Singular match: {key}");
                    continue;
                }

                if (pt.IsArray)
                {
                    // We need to know how many records there are up front, so we need to iterate dictionary
                    var skey = $"{key}[";
                    //var sDic = new Dictionary<string, object>();
                    var highestNum = 0;
                    foreach (var kvp in dic)
                    {
                        if (!kvp.Key.StartsWith(skey))
                            continue;
                        //sDic.Add(kvp.Key, kvp.Value);

                        // Get what number in the array this is
                        var e = 0;
                        for (var s = skey.Length; s < kvp.Key.Length; s++)
                        {
                            if (kvp.Key[s] < 48 || kvp.Key[s] > 57)
                            {
                                e = s - skey.Length;
                                break;
                            }
                        }
                        var num = int.Parse(kvp.Key.Substring(skey.Length, e));
                        if (num > highestNum)
                            highestNum = num;
                        // This is our base key
                        //skey = $"{key}[{num}]";
                    }
                    // Generate array
                    var array = (object[])Activator.CreateInstance(pt, highestNum + 1);
                    property.SetValue(@object, array);

                    // Process all sub-elements
                    for (var i = 0; i <= highestNum; i++)
                    {
                        // If this exists directly then there is no sub-items
                        var sKey = $"{key}[{i}]";
                        if (dic.TryGetValue(sKey, out var dobj))
                        {
                            array[i] = dobj;
                        }
                        else
                        {
                            // If not we continue down the rabbit hole...
                            var sObj = Activator.CreateInstance(pt.GetElementType());
                            array[i] = sObj;
                            UnflattenInt(dic, sKey, array[i]);
                        }
                    }

                    //var list = (object[])property.GetValue(@object);
                    //for (var c = 0; c < list.Length; c++)
                    //    UnflattenInt(dic, $"{key}[{c}]", list[c], memberType);
                    continue;
                }
                if (((IList)ptil).Contains(typeof(IList)))
                {
                    //    var list = (IList)property.GetValue(@object);
                    //    for (var c = 0; c < list.Count; c++)
                    //        UnflattenInt(dic, $"{key}[{c}]", list[c], memberType);
                    continue;
                }
                if (((IList)ptil).Contains(typeof(IEnumerable)))
                {
                    //    var c = 0;
                    //    foreach (var e in (IEnumerable)property.GetValue(@object))
                    //    {
                    //        UnflattenInt(dic, $"{key}[{c}]", e);
                    //        c++;
                    //    }
                    continue;
                }


                var val = property.GetValue(@object);
                // If the value is null we need to create it before we go into it
                if (val is null)
                {
                    val = CreateDefault<T>();
                    property.SetValue(@object, val);
                }

                // Go into property
                UnflattenInt(dic, key, val);
            }
        }

        private T CreateDefault<T>()
        {
            T @object = default;
            if (@object is null)
                @object = Activator.CreateInstance<T>();
            return @object;
        }
    }
#endif