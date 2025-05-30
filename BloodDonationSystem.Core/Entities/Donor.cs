using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Core.Entities;

public class Donor : BaseEntity
{
    public Donor( string name, string email, DateTime birthDate, GenderEnum gender, double weight, TypeBloodEnum typeBlood, RhFactorEnum rhFactor,Address address)
    {
       
        Name = name;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Weight = weight;
        TypeBlood = typeBlood;
        RhFactor = rhFactor;
        Address = address;
    }
    
    protected  Donor()
    {
        
        Name = string.Empty; 
        Email = string.Empty;
        Donations = new List<Donation>();
        Address = null!; 
        
    }


    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public GenderEnum Gender { get; private set; }
    public double Weight { get; private set; }
    public TypeBloodEnum TypeBlood { get; private set; }
    public RhFactorEnum RhFactor { get; private set; }
    public List<Donation> Donations { get; private set; }
    public Address Address { get; private set; }
    public Guid AddressId { get; set; }
}