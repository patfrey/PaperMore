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

        generator.Generate(results, Defaults.DefaultSorting, Defaults.DefaultFilter(cmdArgs), stream);
    }
}