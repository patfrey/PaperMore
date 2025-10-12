using System.CommandLine;
using System.CommandLine.Parsing;

namespace PaperMore.CLI;

public class CmdParser
{
    public bool TryParse(string[] args, out CmdArgs arguments)
    {
        Option<string> urlOption = new Option<string>("--url", ["-u"])
        {
            Description = "URL of you paperless-ngx instance",
            Required = true
        };

        Option<string> tokenOption = new Option<string>("--token", ["-t"])
        {
            Description = "Personal access token for you paperless-ngx instance",
            Required = true
        };

        Option<FormatType> formatOption = new Option<FormatType>("--format", ["-f"])
        {
            Description = "Output type of the report",
            DefaultValueFactory = result => FormatType.Pdf,
            Required = true
        };

        Option<string> pathOption = new Option<string>("--path", ["-p"])
        {
            Description = "Path to write report to",
            Required = true
        };

        Option<int> blankLinesOptions = new Option<int>("--blanklines", ["-b"])
        {
            Description =
                "Number of blank lines to include in the pdf report, use this to add space for manually adding documents",
            DefaultValueFactory = result => 0,
            Required = true
        };
        
        RootCommand rootCommand = new RootCommand("Create a report of all documents in you paperless-ngx instance");
        rootCommand.Add(urlOption);
        rootCommand.Add(tokenOption);
        rootCommand.Add(formatOption);
        rootCommand.Add(pathOption);
        rootCommand.Add(blankLinesOptions);
        
        ParseResult result = rootCommand.Parse(args);

        if (result.Invoke() != 0)
        {
            arguments = null!;
            return false;
        }

        string url = result.GetRequiredValue(urlOption);
        string token = result.GetRequiredValue(tokenOption);
        FormatType format = result.GetRequiredValue(formatOption);
        string pathOutput = result.GetRequiredValue(pathOption);
        int blankLines = result.GetRequiredValue(blankLinesOptions);

        arguments = new CmdArgs(url, token, format, pathOutput, blankLines);
        return true;
    }
}