using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PaperMore.Reports;

public class PdfGenerator : GeneratorBase
{
    public int BlankLines { get; set; }

    public PdfGenerator()
    {
        Settings.License = LicenseType.Community;
        BlankLines = 0;
    }

    protected override void Generate(List<DocumentReportData> data, Stream outputStream)
    {
        long currentAsn = data.Max(d => d.ASN ?? 0);

        var document = Document.Create(doc =>
        {
            doc.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(15);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(style =>
                    style.FontSize(8)
                        .FontFamily("Helvetica")
                );

                page.Header()
                    .Text(text =>
                    {
                        text.DefaultTextStyle(style =>
                            style.SemiBold()
                                .FontSize(16)
                                .FontColor(Colors.Black)
                        );
                        text.Span("Paperless Index ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });

                page.Footer()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Text($"Total documents #{data.Count}")
                            .AlignStart()
                            .Italic()
                            .FontSize(6);
                        row.RelativeItem()
                            .Text($"Current ASN #{currentAsn}")
                            .AlignCenter()
                            .Italic()
                            .FontSize(6);
                        row.RelativeItem()
                            .Text($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToLongTimeString()}")
                            .AlignEnd()
                            .Italic()
                            .FontSize(6);
                    });

                page.Content()
                    .Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(4);
                            cols.RelativeColumn(4);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(2);
                            cols.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(2)
                                .DefaultTextStyle(style => style.Bold().FontSize(10))
                                .Padding(8)
                                .Text("Correspondent");
                            header.Cell().BorderBottom(2)
                                .DefaultTextStyle(style => style.Bold().FontSize(10))
                                .Padding(8)
                                .Text("Title");
                            header.Cell().BorderBottom(2)
                                .DefaultTextStyle(style => style.Bold().FontSize(10))
                                .Padding(8)
                                .AlignCenter().Text("Document Date");
                            header.Cell().BorderBottom(2)
                                .DefaultTextStyle(style => style.Bold().FontSize(10))
                                .Padding(8)
                                .AlignCenter().Text("Entry Added");
                            header.Cell().BorderBottom(2)
                                .DefaultTextStyle(style => style.Bold().FontSize(10))
                                .Padding(8)
                                .AlignRight().Text("ASN");
                        });

                        int rowNumber = 1;
                        const int cellPadding = 4;

                        foreach (DocumentReportData item in data)
                        {
                            Color cellBackground;
                            if (rowNumber % 2 == 0)
                                cellBackground = Colors.White;
                            else
                                cellBackground = Colors.Grey.Lighten1;

                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .Text(item.Correspondent);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .Text(item.Title);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignCenter()
                                .Text(item.DocumentDate.Date.ToShortDateString());
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignCenter()
                                .Text(item.AddedDate.Date.ToShortDateString());
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignRight()
                                .Text(item.ASN is not null ? $"#{item.ASN.ToString()}" : string.Empty);

                            rowNumber++;
                        }

                        // Add blank lines to manually add new documents if set
                        for (int i = 0; i < BlankLines; i++)
                        {
                            Color cellBackground;
                            if (rowNumber % 2 == 0)
                                cellBackground = Colors.White;
                            else
                                cellBackground = Colors.Grey.Lighten1;

                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .Text(string.Empty);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .Text(string.Empty);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignCenter()
                                .Text(string.Empty);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignCenter()
                                .Text(string.Empty);
                            table.Cell().Background(cellBackground)
                                .Padding(cellPadding)
                                .AlignRight()
                                .Text(string.Empty);

                            rowNumber++;
                        }
                    });
            });
        });

        document.GeneratePdf(outputStream);
    }
}