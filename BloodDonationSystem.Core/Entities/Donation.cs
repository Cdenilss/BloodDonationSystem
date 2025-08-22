namespace BloodDonationSystem.Core.Entities;

public class Donation : BaseEntity
{
    public Donation(Guid donorId, DateTime dateDonation, int quantityMl)
    {
        DonorId = donorId;
        DateDonation = dateDonation;
        QuantityMl = quantityMl;
    }

    protected Donation()
    {
    }

    public Guid DonorId { get; private set; }
    public DateTime DateDonation { get; private  set; }
    public int QuantityMl { get; private set; }
    public Donor Donor { get; set; }
}