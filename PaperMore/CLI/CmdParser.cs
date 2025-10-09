using System.CommandLine;
using System.CommandLine.Parsing;

namespace PaperMore.CLI;

public class CmdParser
{
    public bool TryParse(string[] args, out CmdArgs arguments)
    {
        Option<string> urlOption = new Option<string>("--url")
        {
            Description = "URL of you paperless-ngx instance",
            Required = true
        };

        Option<string> tokenOption = new Option<string>("--token")
        {
            Description = "Personal access token for you paperless-ngx instance",
            Required = true
        };

        Option<FormatType> formatOption = new Option<FormatType>("--format")
        {
            Description = "Output type of the report (csv or pdf)",
            DefaultValueFactory = result => FormatType.Pdf,
            Required = true
        };

        Option<string> pathOption = new Option<string>("--path")
        {
            Description = "Path to write report to",
            Required = true
        };

        RootCommand rootCommand = new RootCommand("Create a report of all documents in you paperless-ngx instance");
        rootCommand.Add(urlOption);
        rootCommand.Add(tokenOption);
        rootCommand.Add(formatOption);
        rootCommand.Add(pathOption);
        
        ParseResult result = rootCommand.Parse(args);

        if (result.Errors.Count != 0)
        {
            foreach (ParseError parseError in result.Errors)
            {
                Console.Error.WriteLine(parseError.Message);
            }
            
            // Ignore null here, because if we continue on false it is our own fault
            arguments = null!;
            return false;
        }

        string url = result.GetRequiredValue(urlOption);
        string token = result.GetRequiredValue(tokenOption);
        FormatType format = result.GetRequiredValue(formatOption);
        string pathOutput = result.GetRequiredValue(pathOption);

        arguments = new CmdArgs(url, token, format, pathOutput);
        return true;
    }
}