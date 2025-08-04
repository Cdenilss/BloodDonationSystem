using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Tests;

public class BaseEntityTests
{
    private class TestEntity : BaseEntity { }

    [Fact]
    public void Constructor_ShouldInitializeDefaults()
    {
        var entity = new TestEntity();

        Assert.NotEqual(default, entity.Id);
        Assert.False(entity.IsDeleted);
        Assert.True(entity.CreatedAt <= DateTime.Now);
    }

    [Fact]
    public void SetAsDeleted_ShouldMarkEntityDeleted()
    {
        var entity = new TestEntity();
        entity.SetAsDeleted();
        Assert.True(entity.IsDeleted);
    }
}
