using BloodDonationSystem.Core.Entities;

namespace BloodDonationSystem.Application.Models.DTO;

public class AddressViewModel
{
    public AddressViewModel(string fullAddress, string city, string state, string zipCode)
    {
        FullAddress = fullAddress;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public string FullAddress { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public static AddressViewModel FromEntity(Address address)
    {
        return new AddressViewModel(
            $"{address.Street}, {address.Number} - {address.District}",
            address.City,
            address.State,
            address.ZipCode
        );
    }
}