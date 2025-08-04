using Bogus;
using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Tests;

public class AddressTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        var street = _faker.Address.StreetName();
        var city = _faker.Address.City();
        var state = _faker.Address.State();
        var zip = _faker.Address.ZipCode();
        var number = _faker.Address.BuildingNumber();
        var complement = _faker.Address.SecondaryAddress();
        var district = _faker.Address.County();

        var address = new Address(street, city, state, zip, number, complement, district);

        Assert.Equal(street, address.Street);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(zip, address.ZipCode);
        Assert.Equal(number, address.Number);
        Assert.Equal(complement, address.Complement);
        Assert.Equal(district, address.District);
    }

    [Fact]
    public void Update_ShouldModifyProperties()
    {
        var address = new Address(
            _faker.Address.StreetName(),
            _faker.Address.City(),
            _faker.Address.State(),
            _faker.Address.ZipCode(),
            _faker.Address.BuildingNumber(),
            _faker.Address.SecondaryAddress(),
            _faker.Address.County());

        var newStreet = _faker.Address.StreetName();
        var newCity = _faker.Address.City();
        var newState = _faker.Address.State();
        var newZip = _faker.Address.ZipCode();
        var newNumber = _faker.Address.BuildingNumber();
        var newComplement = _faker.Address.SecondaryAddress();
        var newDistrict = _faker.Address.County();

        address.Update(newStreet, newCity, newState, newZip, newNumber, newComplement, newDistrict);

        Assert.Equal(newStreet, address.Street);
        Assert.Equal(newCity, address.City);
        Assert.Equal(newState, address.State);
        Assert.Equal(newZip, address.ZipCode);
        Assert.Equal(newNumber, address.Number);
        Assert.Equal(newComplement, address.Complement);
        Assert.Equal(newDistrict, address.District);
    }
}
