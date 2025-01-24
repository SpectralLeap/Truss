using Truss.Modeling.Domain.Entities;

namespace Truss.Modeling.Domain.Tests.Unit.Entities;

public sealed class EntityAndAggregateRootIdTests
{
    private sealed record FakeId(Guid value) : AggregateRootId<Guid>(value)
    {
        public Guid value { get; } = value;
    }

    private sealed record SomeId(Guid value) : EntityId<Guid>(value)
    {
        public Guid value { get; } = value;
    }

    [Fact]
    public void AggregateRootIdIsComparableToTId()
    {
        var guid = Guid.NewGuid();

        var fake = new FakeId(guid);
        
        Assert.Equal(guid, fake.value);
    }
    
    [Fact]
    public void ids_of_different_types_are_not_equal_if_value_is_equal()
    {
        var guid = Guid.NewGuid();
    
        var fake = new FakeId(guid);
        var some = new SomeId(guid);
        
        Assert.False(some.Equals(fake));
    }

    [Fact]
    public void ids_of_same_type_have_same_hash_code_with_value()
    {
         var guid = Guid.NewGuid();
     
         var fake1 = new FakeId(guid).GetHashCode();
         var fake2 = new FakeId(guid).GetHashCode();
         
         Assert.Equal(fake1, fake2);       
    }
    
    [Fact]
    public void ids_of_different_types_have_different_hash_code_with_value()
    {
        var guid = Guid.NewGuid();
         
        var fake = new FakeId(guid).GetHashCode();
        var some = new SomeId(guid).GetHashCode();
             
        Assert.NotEqual(fake, some);       
    }
}