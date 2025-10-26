using PaperMore.Api;
using PaperMore.CLI;
using PaperMore.Reports;

namespace PaperMore;

class Program
{
    static void Main(string[] args)
    {
        CmdParser cmdParser = new CmdParser();
        CmdArgs? cmdArgs = null;

        int returnCode = cmdParser.TryParse(args, result => { cmdArgs = result; });
        if (returnCode != 0 || cmdArgs is null)
            Environment.Exit(returnCode);

        IDocumentDataSource source = new DocumentDataSource();
        List<DocumentReportData> results =
            source.GetDocumentData(new DocumentQueryParams(cmdArgs.Url, cmdArgs.Token, cmdArgs.BatchSize));

        results = FilterDocuments(results, cmdArgs);
        SortDocuments(results);

        IReportGenerator generator;

        switch (cmdArgs.Format)
        {
            case FormatType.Csv:
                generator = new CsvGenerator();
                break;
            default:
                generator = new PdfGenerator();
                (generator as PdfGenerator)!.BlankLines = cmdArgs.BlankLines;
                break;
        }

        using FileStream stream = File.Open(cmdArgs.OutputPath, FileMode.Create, FileAccess.Write);

        generator.Generate(results, stream);
    }

    private static List<DocumentReportData> FilterDocuments(List<DocumentReportData> results, CmdArgs args)
    {
        bool isAsnLimited = args.AsnRangeFrom is not null || args.AsnRangeTo is not null;

        if (!isAsnLimited)
            return results;

        var newList = results
            .Where(doc => doc.ASN is not null)
            .ToList();

        newList = newList
            .Where(doc => LongBetween(doc.ASN ?? 0, args.AsnRangeFrom, args.AsnRangeTo))
            .ToList();

        return newList;
    }
    
    private static bool LongBetween(long num, long? lower, long? upper)
    {
        long low = lower ?? 0L;
        long up = upper ?? long.MaxValue;
        
        return low <= num && num <= up;
    }

    private static void SortDocuments(List<DocumentReportData> results)
    {
        results.Sort((lhs, rhs) =>
        {
            int comparison = String.Compare(lhs.Correspondent, rhs.Correspondent,
                StringComparison.CurrentCultureIgnoreCase);

            if (comparison == 0)
                comparison = lhs.DocumentDate.CompareTo(rhs.DocumentDate);

            if (comparison == 0)
                comparison = String.Compare(lhs.Title, rhs.Title, StringComparison.CurrentCultureIgnoreCase);

            return comparison;
        });
    }
}