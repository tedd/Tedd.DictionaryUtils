#if DESERIALIZER || SERIALIZER
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Text;
using Tedd.RandomUtils;

namespace Tedd.DictionaryUtils.Tests.FlattenModels
{
    public class Customer
    {
        public int InternalId = ConcurrentRandom.NextInt32();
        public int Id { get; set; } = ConcurrentRandom.NextInt32();
        public Person ContactPerson { get; set; } = new Person();
        [DoNotFlatten]
        public Timestamp CreatedTs { get; set; } = new Timestamp();
        public List<Order> Orders { get; set; } = new List<Order>() { new Order(), new Order(), new Order(), new Order(), new Order() };
        public Dictionary<string, Person> Employees { get; set; } = new Dictionary<string, Person>() { { "Stig", new Person() }, { "Rob", new Person() } };
        public string[] Statuses { get; set; } = {"One", "Two", "Three"};
    }
}
#endif