using BloodDonationSystem.Application.Models.DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using BloodDonationSystem.Core.Enum;

namespace BloodDonationSystem.Infrastructure.Reports;

public class LastDonationsDetails : IDocument
{
    public IReadOnlyList<DonationViewModel> Donations { get; }

    public LastDonationsDetails(IReadOnlyList<DonationViewModel> donations)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        Donations = donations ?? throw new ArgumentNullException(nameof(donations));
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);

            // Cabeçalho principal
            page.Header().Column(header =>
            {
                header.Item().AlignCenter().Text("Relatório de Doações - Últimos 30 Dias")
                    .FontSize(20).SemiBold().FontColor(Colors.Red.Medium);

                header.Item().AlignCenter().Text($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);

                header.Item().PaddingTop(10)
                    .LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
            });

            // Conteúdo principal
            page.Content().Column(col =>
            {
                // Resumo inicial
                col.Item().PaddingVertical(10).Row(row =>
                {
                    row.RelativeColumn().Text($"Total de Doações: {Donations.Count}")
                        .FontSize(12).Bold();
                    row.RelativeColumn().AlignRight().Text($"Volume Total: {Donations.Sum(d => d.QuantityMl)} ml")
                        .FontSize(12).Bold();
                });

                col.Item().PaddingBottom(10).Text("Detalhes das Doações:")
                    .FontSize(14).SemiBold().FontColor(Colors.Grey.Darken3);

                // Tabela com estilo visual
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(2); 
                        c.RelativeColumn(3); 
                        c.RelativeColumn(3); 
                        c.RelativeColumn(2); 
                    });

                    
                    table.Header(h =>
                    {
                        h.Cell().Element(CellHeader).Text("Data");
                        h.Cell().Element(CellHeader).Text("Doador");
                        h.Cell().Element(CellHeader).Text("Tipo Sanguíneo");
                        h.Cell().Element(CellHeader).Text("Quantidade (ml)");
                    });

                   
                    var isAlt = false;
                    foreach (var d in Donations)
                    {
                        var bgColor = isAlt ? Colors.Grey.Lighten4 : Colors.White;
                        isAlt = !isAlt;

                        table.Cell().Element(c => CellBody(c, bgColor))
                            .Text(d.DateDonation.ToString("dd/MM/yyyy"));
                        table.Cell().Element(c => CellBody(c, bgColor))
                            .Text(d.Donor.Name);
                        table.Cell().Element(c => CellBody(c, bgColor))
                            .Text($"{d.Donor.BloodType}{(d.Donor.RhFactor == RhFactorEnum.Positive ? "+" : "-")}");
                        table.Cell().Element(c => CellBody(c, bgColor))
                            .Text(d.QuantityMl.ToString());
                    }
                });

               
                col.Item().PaddingTop(20).Column(summary =>
                {
                    summary.Item().Text("Resumo Estatístico").FontSize(14).Bold()
                        .FontColor(Colors.Grey.Darken3);

                    var avg = Donations.Count > 0 ? Donations.Average(d => d.QuantityMl) : 0;

                    summary.Item().Text($"🩸 Média de volume por doação: {avg:N0} ml")
                        .FontSize(12);
                    summary.Item().Text($"📅 Período: Últimos 30 dias")
                        .FontSize(12);
                });
            });

            // Rodapé
            page.Footer()
                .AlignRight()
                .Text("Doar Sangue Salva Vidas!")
                .FontSize(10)
                .FontColor(Colors.Grey.Darken1);
        });
    }

    // Estilos auxiliares
    private static IContainer CellHeader(IContainer container) =>
        container.PaddingVertical(5)
            .Background(Colors.Red.Lighten3)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .AlignCenter()
            .AlignMiddle()
            .DefaultTextStyle(x => x.SemiBold().FontSize(11));


    private static IContainer CellBody(IContainer container, string bgColor) =>
        container.Background(bgColor).PaddingVertical(4).PaddingHorizontal(2)
            .AlignMiddle().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3);
}