using Bogus;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Tests;

public class BloodStockTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var bloodType = _faker.PickRandom<BloodTypeEnum>();
        var rhFactor = _faker.PickRandom<RhFactorEnum>();
        var quantity = _faker.Random.Int(100, 1000);

        var stock = new BloodStock(bloodType, rhFactor, quantity, 1500);

        Assert.Equal(bloodType, stock.BloodType);
        Assert.Equal(rhFactor, stock.RhFactor);
        Assert.Equal(quantity, stock.QuantityMl);
    }
}
