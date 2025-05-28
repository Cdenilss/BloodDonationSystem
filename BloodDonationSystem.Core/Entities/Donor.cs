namespace BloodDonationSystem.Core.Entities;

public class Donor : BaseEntity
{
    public Donor(Guid id, string name, string email, DateTime birthDate, string gender, double weight, string typeBlood, string rhFactor, List<Donation> donations, Address address)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Weight = weight;
        TypeBlood = typeBlood;
        RhFactor = rhFactor;
        Donations = donations;
        Address = address;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Gender { get; private set; }
    public double Weight { get; private set; }
    public string TypeBlood { get; private set; }
    public string RhFactor { get; private set; }
    public List<Donation> Donations { get; private set; }
    public Address Address { get; private set; }
    public int AddressId { get; set; }
}