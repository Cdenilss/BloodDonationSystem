using Bogus;
using BloodDonationSystem.Core.Entities;
using BloodDonationSystem.Core.Enum;
using System.Linq;

namespace BloodDonationSystem.Tests;

public class DonorTests
{
    private static readonly Faker Faker = new();

    private Donor CreateDonor()
    {
        var address = new Address(
            Faker.Address.StreetName(),
            Faker.Address.City(),
            Faker.Address.State(),
            Faker.Address.ZipCode(),
            Faker.Address.BuildingNumber(),
            Faker.Address.SecondaryAddress(),
            Faker.Address.County());

        return new Donor(
            Faker.Person.FullName,
            Faker.Internet.Email(),
            Faker.Date.Past(30, DateTime.Now.AddYears(-18)),
            Faker.PickRandom<GenderEnum>(),
            Faker.Random.Double(50, 100),
            Faker.PickRandom<BloodTypeEnum>(),
            Faker.PickRandom<RhFactorEnum>(),
            address);
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyDonationsList()
    {
        var donor = CreateDonor();

        Assert.NotNull(donor.Donations);
        Assert.Empty(donor.Donations);
    }

    [Fact]
    public void Update_ShouldChangeNameAndEmail()
    {
        var donor = CreateDonor();
        var newName = Faker.Person.FullName;
        var newEmail = Faker.Internet.Email();

        donor.Update(newName, newEmail);

        Assert.Equal(newName, donor.Name);
        Assert.Equal(newEmail, donor.Email);
    }

    [Fact]
    public void AddingDonation_ShouldIncreaseDonationCount()
    {
        var donor = CreateDonor();
        var donation = new Donation(donor.Id, Faker.Date.Recent(), Faker.Random.Int(100, 500));

        donor.Donations.Add(donation);

        Assert.Single(donor.Donations);
        Assert.Equal(donor.Id, donor.Donations.First().DonorId);
    }
}
