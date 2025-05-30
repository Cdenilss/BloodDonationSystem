namespace BloodDonationSystem.Core.Entities;

public class Address : BaseEntity
{
    public Address(string street, string city, string state, string zipCode, Donor donor)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Donor = donor;
    }

    public Guid Id { get;  private set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    
    public int DonorId { get; set; }
    public Donor Donor { get; set; }
    
}