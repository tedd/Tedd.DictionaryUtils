using System;
using System.Collections.Generic;
using System.Linq;
using Tedd.RandomUtils;
using Xunit;

namespace Tedd.DictionaryUtils.Tests
{
    public class ToDictionarySafeTest
    {
        private const int ListSize = 1000;
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private FastRandom _rnd = new FastRandom();

        private struct KV<TKey, TValue>
        {
            public TKey Key;
            public TValue Value;

            #region Overrides of ValueType

            /// <summary>Indicates whether this instance and a specified object are equal.</summary>
            /// <param name="obj">The object to compare with the current instance.</param>
            /// <returns>
            /// <see langword="true" /> if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
            public override bool Equals(object? obj)
            {
                var ob = obj as KV<TKey, TValue>?;
                if (ob == null)
                    return false;
                var o = ob.Value;
                return Key!.Equals(o.Key) && Value!.Equals(o.Value);
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash = hash * 31 + (Key == null ? 0 : Key.GetHashCode());
                hash = hash * 31 + (Value == null ? 0 : Value.GetHashCode());
                return hash;
            }

            #endregion
        }

        private void SetUpLists(out List<KV<string, int>> singleList, out List<KV<string, int>> dupList)
        {
            singleList = new List<KV<string, int>>(ListSize);
            dupList = new List<KV<string, int>>(ListSize * 2);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = new KV<string, int>()
                {
                    Key = _rnd.NextString(Letters, 10),
                    Value = i
                };
                singleList.Add(kv);
                dupList.Add(kv);
                if (i % 3 == 0)
                    dupList.Add(kv);
            }
        }

        private void VerifyListsKey(List<KV<string, int>> singleList, Dictionary<string, KV<string, int>> dic)
        {
            Assert.Equal(singleList.Count, dic.Count);
            for (var i = 0; i < singleList.Count; i++)
            {
                var kv = singleList[i];
                Assert.True(dic.TryGetValue(kv.Key, out var val));
                Assert.Equal(kv.Value, val.Value);
            }
        }
        private void VerifyListsKeyValue(List<KV<string, int>> singleList, Dictionary<string, int> dic)
        {
            Assert.Equal(dic.Count, singleList.Count);
            for (var i = 0; i < ListSize; i++)
            {
                var kv = singleList[i];
                Assert.True(dic.TryGetValue(kv.Key, out var val));
                Assert.Equal(kv.Value, val);
            }
        }

        #region List
        [Fact]
        public void ListToDictionarySafeKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToDictionarySafe(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ListToDictionarySafeKeyValue()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToDictionarySafe(k => k.Key, v => v.Value);
            VerifyListsKeyValue(singleList, dic);
        }
        [Fact]
        public void ListToDictionarySafeKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToDictionarySafe(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ListToDictionarySafeKeyValueComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToDictionarySafe(k => k.Key.ToLowerInvariant(), v => v.Value, StringComparer.OrdinalIgnoreCase);
            VerifyListsKeyValue(singleList, dic);
        }
        #endregion
        #region Array
        [Fact]
        public void ArrayToDictionarySafeKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToDictionarySafe(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ArrayToDictionarySafeKeyValue()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToDictionarySafe(k => k.Key, v => v.Value);
            VerifyListsKeyValue(singleList, dic);
        }
        [Fact]
        public void ArrayToDictionarySafeKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToDictionarySafe(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void ArrayToDictionarySafeKeyValueComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToArray().ToDictionarySafe(k => k.Key.ToLowerInvariant(), v => v.Value, StringComparer.OrdinalIgnoreCase);
            VerifyListsKeyValue(singleList, dic);
        }
        #endregion
        #region IEnumerable (HashSet)
        [Fact]
        public void IEnumerableToDictionarySafeKey()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToDictionarySafe(k => k.Key);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void IEnumerableToDictionarySafeKeyValue()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToDictionarySafe(k => k.Key, v => v.Value);
            VerifyListsKeyValue(singleList, dic);
        }
        [Fact]
        public void IEnumerableToDictionarySafeKeyComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToDictionarySafe(k => k.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase);
            VerifyListsKey(singleList, dic);
        }

        [Fact]
        public void IEnumerableToDictionarySafeKeyValueComparer()
        {
            SetUpLists(out var singleList, out var dupList);
            var dic = dupList.ToHashSet().ToDictionarySafe(k => k.Key.ToLowerInvariant(), v => v.Value, StringComparer.OrdinalIgnoreCase);
            VerifyListsKeyValue(singleList, dic);
        }
        #endregion

        #region Empty Collections
        [Fact]
        public void ListToDictionarySafe_Empty()
        {
            var empty = new List<KV<string, int>>();
            var dic1 = empty.ToDictionarySafe(k => k.Key);
            var dic2 = empty.ToDictionarySafe(k => k.Key, v => v.Value);
            Assert.Empty(dic1);
            Assert.Empty(dic2);
        }

        [Fact]
        public void ArrayToDictionarySafe_Empty()
        {
            var empty = Array.Empty<KV<string, int>>();
            var dic1 = empty.ToDictionarySafe(k => k.Key);
            var dic2 = empty.ToDictionarySafe(k => k.Key, v => v.Value);
            Assert.Empty(dic1);
            Assert.Empty(dic2);
        }

        [Fact]
        public void IEnumerableToDictionarySafe_Empty()
        {
            var empty = Enumerable.Empty<KV<string, int>>();
            var dic1 = empty.ToDictionarySafe(k => k.Key);
            var dic2 = empty.ToDictionarySafe(k => k.Key, v => v.Value);
            Assert.Empty(dic1);
            Assert.Empty(dic2);
        }
        #endregion

        #region Null Arguments
        [Fact]
        public void ToDictionarySafe_NullSource_Throws()
        {
            List<KV<string, int>> nullList = null!;
            Assert.Throws<ArgumentException>(() => nullList.ToDictionarySafe(k => k.Key));
            Assert.Throws<ArgumentException>(() => nullList.ToDictionarySafe(k => k.Key, v => v.Value));
        }

        [Fact]
        public void ToDictionarySafe_NullKeySelector_Throws()
        {
            var list = new List<KV<string, int>>();
            Assert.Throws<ArgumentException>(() => list.ToDictionarySafe<KV<string, int>, string>(null!));
            Assert.Throws<ArgumentException>(() => list.ToDictionarySafe<KV<string, int>, string, int>(null!, v => v.Value));
        }

        [Fact]
        public void ToDictionarySafe_NullElementSelector_Throws()
        {
            var list = new List<KV<string, int>>();
            Assert.Throws<ArgumentException>(() => list.ToDictionarySafe(k => k.Key, (Func<KV<string, int>, int>)null!));
        }
        #endregion

    }
}
