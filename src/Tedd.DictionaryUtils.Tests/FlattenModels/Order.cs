using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class Order
    {
        public int Id { get; set; } = ConcurrentRandom.NextInt32();
        public Product Product { get; set; } = new Product();
    }
}