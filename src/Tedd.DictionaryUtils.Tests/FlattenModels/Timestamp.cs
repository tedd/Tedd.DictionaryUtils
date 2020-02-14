using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class Timestamp
    {
        public int TSValue { get; set; } = ConcurrentRandom.NextInt32();
    }
}