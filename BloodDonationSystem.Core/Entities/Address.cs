namespace BloodDonationSystem.Core.Entities;

public class Address : BaseEntity
{
    public Address(string street, string city, string state, string zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        
    }
    protected Address() { }

 
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private  set; }
    public string ZipCode { get; private  set; }
    
    public Guid DonorId { get; private  set; }
    public Donor? Donor { get; private  set; }
    
}