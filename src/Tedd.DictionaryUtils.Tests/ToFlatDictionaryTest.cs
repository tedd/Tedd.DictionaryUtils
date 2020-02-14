using System;
using System.Collections.Generic;
using System.Text;
using Tedd.DictionaryUtils.Tests.FlattenModels;
using Xunit;

namespace Tedd.DictionaryUtils.Tests
{
    public class ToFlatDictionaryTest
    {

        [Fact]
        public void FlattenObject()
        {
            var customer = new Customer();
            var flattener = new DictionarySerializer();
            var dic = flattener.ToFlatDictionary(customer, FlattenMemberType.Property);
            // DoNotFlatten
            Assert.True(dic.ContainsKey(nameof(customer.CreatedTs)));
            // Plain property
            Assert.True(dic.ContainsKey(nameof(customer.Id)));
            // Person object goes deeper
            Assert.False(dic.ContainsKey(nameof(customer.ContactPerson)));
            Assert.True(dic.ContainsKey(nameof(customer.ContactPerson) + "." + nameof(customer.ContactPerson.Name)));
            Assert.True(dic.ContainsKey(nameof(customer.ContactPerson) + "." + nameof(customer.ContactPerson.Address)));
            Assert.True(dic.ContainsKey(nameof(customer.ContactPerson) + "." + nameof(customer.ContactPerson.Created)));
        }
    }
}
