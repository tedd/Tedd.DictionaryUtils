using System;
using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class Person
    {
        public string Name { get; set; } = "Name:" + ConcurrentRandom.NextString("abcdefghABCDEFGH", 4);
        public string Address { get; set; } = "Address:" + ConcurrentRandom.NextString("abcdefghABCDEFGH", 4);
        public DateTime Created { get; set; } = new DateTime(2000,01,01);
    }
}