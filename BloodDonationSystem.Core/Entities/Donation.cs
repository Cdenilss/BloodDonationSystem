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

    public Guid DonorId { get; set; }
    public DateTime DateDonation { get; set; }
    public int QuantityMl { get; set; }
    public Donor Donor { get; set; }
}