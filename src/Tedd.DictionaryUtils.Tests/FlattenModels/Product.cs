using System.Collections.Generic;
using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class Product
    {
        public string Name { get; set; } = ConcurrentRandom.NextString("abcdefghABCDEFGH0123456789", 10);
        public List<ProductPrice> PriceHistory { get; set; } = new List<ProductPrice>() { new ProductPrice(), new ProductPrice(), new ProductPrice(), new ProductPrice() };
    }
}