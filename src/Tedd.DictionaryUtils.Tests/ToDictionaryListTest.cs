using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tedd.DictionaryUtils.Tests
{
    public class ToDictionaryListTest
    {
        private class Item
        {
            public int Id { get; set; }
            public string Category { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
        }

        private readonly List<Item> _testItems = new List<Item>
        {
            new Item { Id = 1, Category = "A", Name = "One" },
            new Item { Id = 2, Category = "B", Name = "Two" },
            new Item { Id = 3, Category = "A", Name = "Three" },
            new Item { Id = 4, Category = "C", Name = "Four" },
            new Item { Id = 5, Category = "B", Name = "Five" }
        };

        private class TestCollection<T> : System.Collections.ObjectModel.Collection<T>
        {
            public TestCollection(IList<T> list) : base(list) { }
        }

        [Fact]
        public void ToDictionaryList_KeySelector_CollectionFallback()
        {
            var collection = new TestCollection<Item>(_testItems);
            var dict = collection.ToDictionaryList(i => i.Category);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_CollectionFallback()
        {
            var collection = new TestCollection<Item>(_testItems);
            var dict = collection.ToDictionaryList(i => i.Category, i => i.Name);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_List()
        {
            var dict = _testItems.ToDictionaryList(i => i.Category);

            Assert.Equal(3, dict.Count);
            Assert.True(dict.ContainsKey("A"));
            Assert.True(dict.ContainsKey("B"));
            Assert.True(dict.ContainsKey("C"));
            Assert.Equal(2, dict["A"].Count);
            Assert.Equal(2, dict["B"].Count);
            Assert.Single(dict["C"]);
            Assert.Equal(1, dict["A"][0].Id);
            Assert.Equal(3, dict["A"][1].Id);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_Array()
        {
            var arr = _testItems.ToArray();
            var dict = arr.ToDictionaryList(i => i.Category);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_IEnumerable()
        {
            IEnumerable<Item> seq = _testItems.Select(x => x);
            var dict = seq.ToDictionaryList(i => i.Category);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_Comparer_List()
        {
            var items = new List<Item> { new Item { Category = "a" }, new Item { Category = "A" } };
            var dict = items.ToDictionaryList(i => i.Category, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_Comparer_Array()
        {
            var items = new Item[] { new Item { Category = "a" }, new Item { Category = "A" } };
            var dict = items.ToDictionaryList(i => i.Category, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeySelector_Comparer_IEnumerable()
        {
            IEnumerable<Item> items = new List<Item> { new Item { Category = "a" }, new Item { Category = "A" } }.Select(x => x);
            var dict = items.ToDictionaryList(i => i.Category, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_List()
        {
            var dict = _testItems.ToDictionaryList(i => i.Category, i => i.Name);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
            Assert.Equal("One", dict["A"][0]);
            Assert.Equal("Three", dict["A"][1]);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_Array()
        {
            var arr = _testItems.ToArray();
            var dict = arr.ToDictionaryList(i => i.Category, i => i.Name);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_IEnumerable()
        {
            IEnumerable<Item> seq = _testItems.Select(x => x);
            var dict = seq.ToDictionaryList(i => i.Category, i => i.Name);

            Assert.Equal(3, dict.Count);
            Assert.Equal(2, dict["A"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_Comparer_List()
        {
            var items = new List<Item> { new Item { Category = "a", Name = "X" }, new Item { Category = "A", Name = "Y" } };
            var dict = items.ToDictionaryList(i => i.Category, i => i.Name, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
            Assert.Equal("X", dict["a"][0]);
            Assert.Equal("Y", dict["a"][1]);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_Comparer_Array()
        {
            var items = new Item[] { new Item { Category = "a", Name = "X" }, new Item { Category = "A", Name = "Y" } };
            var dict = items.ToDictionaryList(i => i.Category, i => i.Name, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
        }

        [Fact]
        public void ToDictionaryList_KeyValueSelector_Comparer_IEnumerable()
        {
            IEnumerable<Item> items = new List<Item> { new Item { Category = "a", Name = "X" }, new Item { Category = "A", Name = "Y" } }.Select(x => x);
            var dict = items.ToDictionaryList(i => i.Category, i => i.Name, StringComparer.OrdinalIgnoreCase);

            Assert.Single(dict);
            Assert.Equal(2, dict["a"].Count);
        }

        [Fact]
        public void ToDictionaryList_EmptyList()
        {
            var empty = new List<Item>();
            var dict = empty.ToDictionaryList(i => i.Category);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_EmptyArray()
        {
            var empty = Array.Empty<Item>();
            var dict = empty.ToDictionaryList(i => i.Category);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_EmptyIEnumerable()
        {
            IEnumerable<Item> empty = Enumerable.Empty<Item>();
            var dict = empty.ToDictionaryList(i => i.Category);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_EmptyList_KeyValue()
        {
            var empty = new List<Item>();
            var dict = empty.ToDictionaryList(i => i.Category, i => i.Name);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_EmptyArray_KeyValue()
        {
            var empty = Array.Empty<Item>();
            var dict = empty.ToDictionaryList(i => i.Category, i => i.Name);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_EmptyIEnumerable_KeyValue()
        {
            IEnumerable<Item> empty = Enumerable.Empty<Item>();
            var dict = empty.ToDictionaryList(i => i.Category, i => i.Name);
            Assert.Empty(dict);
        }

        [Fact]
        public void ToDictionaryList_NullSource_Throws()
        {
            List<Item> nullList = null!;
            Assert.Throws<ArgumentNullException>(() => nullList.ToDictionaryList(i => i.Category));
            Assert.Throws<ArgumentNullException>(() => nullList.ToDictionaryList(i => i.Category, i => i.Name));
        }

        [Fact]
        public void ToDictionaryList_NullKeySelector_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _testItems.ToDictionaryList<Item, string>(null!));
            Assert.Throws<ArgumentNullException>(() => _testItems.ToDictionaryList<Item, string, string>(null!, i => i.Name));
        }

        [Fact]
        public void ToDictionaryList_NullElementSelector_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _testItems.ToDictionaryList(i => i.Category, (Func<Item, string>)null!));
        }
    }
}
