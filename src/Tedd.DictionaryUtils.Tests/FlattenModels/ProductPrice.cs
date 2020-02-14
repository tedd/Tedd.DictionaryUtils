using System;
using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class ProductPrice
    {
        public DateTime StartPeriod { get; set; } = new DateTime(2001,01,01 + ConcurrentRandom.Next(0, 28));
        public DateTime EndPeriod { get; set; } = new DateTime(2002,01,01 + ConcurrentRandom.Next(0, 28));
        public int Price { get; set; } = ConcurrentRandom.NextInt32();
    }
}