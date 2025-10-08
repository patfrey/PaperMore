using System.Text;

namespace PaperMore.Reports;

public class CsvGenerator : IReportGenerator
{
    public void Generate(List<DocumentReportData> data, Stream outputStream)
    {
        const string TextDelimiter = "\"";
        
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Correspondent,Title,DocumentDate,Added,ASN");

        var last = data.Last();
        foreach (DocumentReportData document in data)
        {
            string asn = document.ASN.ToString() ?? string.Empty;
            
            builder.Append(TextDelimiter)
                .Append(document.Correspondent)
                .Append(TextDelimiter)
                .Append(",")
                .Append(TextDelimiter)
                .Append(document.Title)
                .Append(TextDelimiter)
                .Append(",")
                .Append(TextDelimiter)
                .Append(document.DocumentDate.Date.ToShortDateString())
                .Append(TextDelimiter)
                .Append(",")
                .Append(TextDelimiter)
                .Append(document.AddedDate.Date.ToShortDateString())
                .Append(TextDelimiter)
                .Append(",")
                .AppendLine(asn);
        }
        
        using StreamWriter streamWriter = new StreamWriter(outputStream);
        streamWriter.WriteLine(builder.ToString());
    }
}