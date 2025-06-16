namespace BloodDonationSystem.Core.Entities;

public class Address : BaseEntity
{
    public Address(string street, string city, string state, string zipCode, string number, string? complement, string district)
        : base()
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Number = number;
        Complement = complement;
        District = district;
        
    }
    
    protected Address() { }

 
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private  set; }
    public string ZipCode { get; private  set; }

    public string? Number { get; private set; }
    public string? Complement { get; private set; }
    public string District { get; private set;}

    public Guid DonorId { get; private  set; }
    public Donor? Donor { get; private  set; }
    
    public void Update(string street, string city, string state, string zipCode, string number, string? complement, string district)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Number = number;
        Complement = complement;
        District = district;
    }
}