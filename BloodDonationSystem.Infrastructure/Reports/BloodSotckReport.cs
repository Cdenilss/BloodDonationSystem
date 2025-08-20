using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Core.Enum;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace BloodDonationSystem.Infrastructure.Reports;

public class BloodStockReport : IDocument
{
    public IReadOnlyList<BloodStockItemViewModel> Stocks { get; }

    public BloodStockReport(IReadOnlyList<BloodStockItemViewModel> stocks)
    {
        Stocks = stocks;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);

            page.Header()
                .AlignCenter()
                .Text("Blood Stock Report")
                .FontSize(20)
                .SemiBold();

            page.Content().Column(col =>
            {
                foreach (var item in Stocks)
                {
                    var typeLabel =
                        $"Tipo sanguíneo {item.BloodType}{(item.RhFactor == RhFactorEnum.Positive ? "+" : "-")}";
                    col.Item().Text($"{typeLabel} : {item.QuantityMl} ml").FontSize(14);
                }

                col.Item().PaddingTop(10).Text($"Total geral: {Stocks.Sum(s => s.QuantityMl)} ml").SemiBold();
            });

            page.Footer()
                .AlignRight()
                .Text($"Generated at {DateTime.Now:dd/MM/yyyy HH:mm}");
        });
    }
}