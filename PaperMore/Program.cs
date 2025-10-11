using PaperMore.Api;
using PaperMore.CLI;
using PaperMore.Reports;

namespace PaperMore;

class Program
{
    static void Main(string[] args)
    {
        CmdParser cmdParser = new CmdParser();

        if(!cmdParser.TryParse(args, out var cmdArgs))
            Environment.Exit(1);
        
        DocumentDataSource source = new DocumentDataSource();
        List<DocumentReportData> results = source.GetDocumentData(cmdArgs.Url, cmdArgs.Token);
        
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

    private static void SortDocuments(List<DocumentReportData> results)
    {
        results.Sort((lhs, rhs) =>
        {
            int comparison = String.Compare(lhs.Correspondent, rhs.Correspondent, StringComparison.CurrentCultureIgnoreCase);
            
            if(comparison == 0)
                comparison = lhs.DocumentDate.CompareTo(rhs.DocumentDate);
                
            if(comparison == 0)
                comparison = String.Compare(lhs.Title, rhs.Title, StringComparison.CurrentCultureIgnoreCase);

            return comparison;
        });
    }
}