using System.Globalization;
using BloodDonationSystem.Application.Models.DTO;
using BloodDonationSystem.Core.Enum;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BloodDonationSystem.Infrastructure.Reports;

public class BloodStockReport : IDocument
{
    public IReadOnlyList<BloodStockItemViewModel> Stocks { get; }

    public BloodStockReport(IReadOnlyList<BloodStockItemViewModel> stocks)
    {
        Stocks = stocks ?? throw new ArgumentNullException(nameof(stocks));
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Size(PageSizes.A4);

            // Cabeçalho
            page.Header().Column(header =>
            {
                header.Item().AlignCenter().Text("Relatório de Estoque de Sangue")
                    .FontSize(20).SemiBold().FontColor(Colors.Red.Medium);

                header.Item().AlignCenter().Text($"Gerado em {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);

                header.Item().PaddingTop(10)
                    .LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
            });

            page.Content().Column(col =>
            {
                // KPIs/Resumo
                var totalMl = Stocks.Sum(s => s.QuantityMl);
                var totalTipos = Stocks.Count;
                var abaixoMinimo = Stocks.Count(s => s.QuantityMl <= s.MinimumSafeQuantity);

                col.Item().PaddingVertical(10).Row(row =>
                {
                    row.RelativeColumn().Text($"Itens de estoque: {totalTipos}")
                        .FontSize(12).Bold();
                    row.RelativeColumn().AlignCenter().Text($"Total geral: {totalMl:N0} ml")
                        .FontSize(12).Bold();
                    row.RelativeColumn().AlignRight().Text($"Abaixo do mínimo: {abaixoMinimo}")
                        .FontSize(12).Bold().FontColor(abaixoMinimo > 0 ? Colors.Red.Medium : Colors.Green.Darken1);
                });

                col.Item().PaddingBottom(10).Text("Detalhamento por Tipo Sanguíneo")
                    .FontSize(14).SemiBold().FontColor(Colors.Grey.Darken3);

                // Tabela
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(2);  // Tipo
                        c.RelativeColumn(1);  // Rh
                        c.RelativeColumn(2);  // Quantidade
                        c.RelativeColumn(2);  // Mínimo
                        c.RelativeColumn(4);  // Progresso (barra)
                        c.RelativeColumn(2);  // Status
                    });

                    // Cabeçalho
                    table.Header(h =>
                    {
                        h.Cell().Element(CellHeader).Text("Tipo");
                        h.Cell().Element(CellHeader).Text("Rh");
                        h.Cell().Element(CellHeader).Text("Quantidade (ml)");
                        h.Cell().Element(CellHeader).Text("Mínimo (ml)");
                        h.Cell().Element(CellHeader).Text("Progresso");
                        h.Cell().Element(CellHeader).Text("Status");
                    });

                    // Linhas
                    var isAlt = false;
                    foreach (var s in Stocks.OrderBy(x => x.BloodType).ThenBy(x => x.RhFactor))
                    {
                        var bgColor = isAlt ? Colors.Grey.Lighten4 : Colors.White;
                        isAlt = !isAlt;

                        var tipo = EnumToLabel(s.BloodType);
                        var rh = s.RhFactor == RhFactorEnum.Positive ? "+" : "-";
                        var statusLow = s.QuantityMl <= s.MinimumSafeQuantity;

                        table.Cell().Element(c => CellBody(c, bgColor)).Text(tipo);
                        table.Cell().Element(c => CellBody(c, bgColor)).Text(rh);
                        table.Cell().Element(c => CellBody(c, bgColor)).AlignRight().Text($"{s.QuantityMl:N0}");
                        table.Cell().Element(c => CellBody(c, bgColor)).AlignRight().Text($"{s.MinimumSafeQuantity:N0}");

                        
                        var ratio = s.MinimumSafeQuantity <= 0 ? 1.0
                            : Math.Min(1.0, (double)s.QuantityMl / s.MinimumSafeQuantity);

                        table.Cell().Element(c => CellBody(c, bgColor)).PaddingRight(6).Element(ProgressBar(ratio, statusLow));

                        table.Cell().Element(c => CellBody(c, bgColor)).Text(statusLow ? "Abaixo do mínimo" : "OK")
                            .FontColor(statusLow ? Colors.Red.Medium : Colors.Green.Darken1)
                            .SemiBold();
                    }

                    
                    table.Cell().ColumnSpan(2).Element(TotalCell).Text("Totais");
                    table.Cell().Element(TotalCell).AlignRight().Text($"{Stocks.Sum(s => s.QuantityMl):N0}");
                    table.Cell().Element(TotalCell).AlignRight().Text($"{Stocks.Sum(s => s.MinimumSafeQuantity):N0}");
                    table.Cell().ColumnSpan(2).Element(TotalCell).AlignRight()
                         .Text(abaixoMinimo > 0 ? $"⚠ {abaixoMinimo} item(ns) abaixo do mínimo" : "Todos acima do mínimo");
                });

                
                col.Item().PaddingTop(16).Text(txt =>
                {
                    txt.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken2));
                    txt.Span("Observação: ").SemiBold();
                    txt.Span("a barra de progresso compara a quantidade atual ao mínimo seguro. Quando a barra atinge 100%, o estoque está no patamar mínimo ou acima.");
                });

            });

            // Paginação - CORRIGIDO
            page.Footer()
                .AlignRight()
                .Text(t =>
                {
                    t.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken1));
                    t.Span("Página ");
                    t.CurrentPageNumber();
                    t.Span(" de ");
                    t.TotalPages();
                });
        });
    }
    
    private static IContainer CellHeader(IContainer container) =>
        container.PaddingVertical(6).Background(Colors.Red.Lighten3)
            .BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
            .AlignCenter().AlignMiddle().DefaultTextStyle(x => x.SemiBold().FontSize(11));

    private static IContainer CellBody(IContainer container, string bgColor) =>
        container.Background(bgColor).PaddingVertical(4).PaddingHorizontal(4)
            .AlignMiddle().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten3);
    private static IContainer TotalCell(IContainer container) =>
        container.Background(Colors.Grey.Lighten4)
            .PaddingVertical(6).PaddingHorizontal(4)
            .BorderTop(1).BorderColor(Colors.Grey.Lighten2)
            .DefaultTextStyle(t => t.SemiBold());

    private static Action<IContainer> ProgressBar(double ratio, bool isLow) => container =>
    {
        var pct = Math.Clamp(ratio, 0, 1);
        var barColor = isLow ? Colors.Orange.Medium : Colors.Green.Medium;

        container
            .Height(10)
            .Border(1).BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.White)
            .Row(row =>
            {
                var fillPercentage = (float)Math.Clamp(pct * 100, 0, 100);
    
                if (fillPercentage > 0)
                {
                    row.RelativeItem(fillPercentage).Background(barColor);
                }
    
                if (fillPercentage < 100)
                {
                    row.RelativeItem(100 - fillPercentage).Background(Colors.White);
                }
            });

    };

    private static string EnumToLabel(BloodTypeEnum type) => type switch
    {
        BloodTypeEnum.O => "O",
        BloodTypeEnum.A => "A",
        BloodTypeEnum.B => "B",
        BloodTypeEnum.AB => "AB",
        _ => type.ToString()
    };
}
