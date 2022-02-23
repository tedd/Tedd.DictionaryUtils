#if DESERIALIZER
using Tedd.DictionaryUtils.Tests.FlattenModels;
using Xunit;

namespace Tedd.DictionaryUtils.Tests;

public class DictionaryDeserializer
{

    [Fact]
    public void UnflattenObject()
    {
        var customer = new Customer();
        var flattener = new Tedd.DictionarySerializer();
        var dic = flattener.ToFlatDictionary(customer, FlattenMemberType.Property);
        var unflattener = new Tedd.DictionaryDeserializer();
        var customer2 = unflattener.UnflattenDictionary<Customer>(dic);

        Assert.Equal(customer.Id, customer2.Id);
        Assert.Equal(customer.CreatedTs.TSValue, customer2.CreatedTs.TSValue);
        Assert.Equal(customer.ContactPerson.Name, customer2.ContactPerson.Name);
        Assert.Equal(customer.ContactPerson.Address, customer2.ContactPerson.Address);


    }
}
#endif