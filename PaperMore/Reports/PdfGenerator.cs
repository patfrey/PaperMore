using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PaperMore.Reports;

public class PdfGenerator : IReportGenerator
{
    public PdfGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public void Generate(List<DocumentReportData> data, Stream outputStream)
    {
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
                                .FontSize(20)
                                .FontColor(Colors.Black)
                        );
                        text.Span("Paperless Index ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });

                page.Footer()
                    .Text($"{DateTime.Now.ToShortDateString()}, {DateTime.Now.ToLongTimeString()}")
                    .AlignEnd()
                    .Italic()
                    .FontSize(6);


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
                        const int CellPadding = 4;
               
                        foreach (DocumentReportData item in data)
                        {
                            Color cellBackground;
                            if (rowNumber % 2 == 0)
                                cellBackground = Colors.White;
                            else
                                cellBackground = Colors.Grey.Lighten1;
                            
                            table.Cell().Background(cellBackground)
                                .Padding(CellPadding)
                                .Text(item.Correspondent);
                            table.Cell().Background(cellBackground)
                                .Padding(CellPadding)
                                .Text(item.Title);
                            table.Cell().Background(cellBackground)
                                .Padding(CellPadding)
                                .AlignCenter()
                                .Text(item.DocumentDate.Date.ToShortDateString());
                            table.Cell().Background(cellBackground)
                                .Padding(CellPadding)
                                .AlignCenter()
                                .Text(item.AddedDate.Date.ToShortDateString());
                            table.Cell().Background(cellBackground)
                                .Padding(CellPadding)
                                .AlignRight()
                                .Text(item.ASN is not null ? $"#{item.ASN.ToString()}" : string.Empty);

                            rowNumber++;
                        }
                    });
            });
        });

        document.GeneratePdf(outputStream);
    }
}