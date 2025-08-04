using Bogus;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Tests;

public class DonationTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var donorId = Guid.NewGuid();
        var date = _faker.Date.Recent();
        var quantity = _faker.Random.Int(100, 500);

        var donation = new Donation(donorId, date, quantity);

        Assert.Equal(donorId, donation.DonorId);
        Assert.Equal(date, donation.DateDonation);
        Assert.Equal(quantity, donation.QuantityMl);
    }
}
