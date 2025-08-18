using BloodDonationSystem.Application.Models.DTO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BloodDonationSystem.Infrastructure.Reports;

public class LastDonationsDetails : IDocument
{
    public IReadOnlyList<DonationViewModel> Donations { get; }

    public LastDonationsDetails(IReadOnlyList<DonationViewModel> donations)
    {
        QuestPDF.Settings.License= LicenseType.Community;
        Donations = donations ?? throw new ArgumentNullException(nameof(donations));
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Header().AlignCenter().Text("Donation Report - Last 30 days")
                .FontSize(20).SemiBold();

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(c =>
                {
                    c.RelativeColumn(2); // Date
                    c.RelativeColumn(3); // Donor
                    c.RelativeColumn(4); // Email
                    c.RelativeColumn(2); // Blood
                    c.RelativeColumn(2); // Ml
                });

                table.Header(h =>
                {
                    h.Cell().Text("Date").SemiBold();
                    h.Cell().Text("Donor").SemiBold();
                    h.Cell().Text("Blood").SemiBold();
                    h.Cell().Text("RhFactor").SemiBold();
                    h.Cell().Text("Qty (ml)").SemiBold();
                   
                });

                foreach (var d in Donations)
                {
                    table.Cell().Text(d.DateDonation.ToString("dd/MM/yyyy"));
                    table.Cell().Text(d.Donor.Name);
                    table.Cell().Text(d.Donor.BloodType);
                    table.Cell().Text(d.Donor.RhFactor);
                    table.Cell().Text(d.QuantityMl.ToString());
                    
                    
                }
                table.Cell().Text("Total").SemiBold();
                table.Cell().Text(Donations.Count.ToString()).SemiBold();
                table.Cell().Text("Total Blood").SemiBold();
                table.Cell().Text(Donations.Sum(d => d.QuantityMl).ToString()).SemiBold();
                table.Cell().Text("ml").SemiBold();
                
                
            });

            page.Footer().AlignRight().Text($"Generated at {DateTime.Now:dd/MM/yyyy HH:mm}");
        });
    }
}