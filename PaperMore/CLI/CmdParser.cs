using System.CommandLine;

namespace PaperMore.CLI;

public class CmdParser
{
    public int TryParse(string[] args, Action<CmdArgs> callback)
    {
        Option<string> urlOption = new Option<string>("--url", "-u")
        {
            Description = "URL of you paperless-ngx instance",
            Required = true
        };

        Option<string> tokenOption = new Option<string>("--token", "-t")
        {
            Description = "Personal access token for you paperless-ngx instance",
            Required = true
        };

        Option<FormatType> formatOption = new Option<FormatType>("--format", "-f")
        {
            Description = "Output type of the report",
            DefaultValueFactory = result => FormatType.Pdf,
            Required = true
        };

        Option<string> pathOption = new Option<string>("--path", "-p")
        {
            Description = "Path to write report to",
            Required = true
        };

        Option<int> blankLinesOptions = new Option<int>("--blanklines", "-b")
        {
            Description =
                "Number of blank lines to include in the pdf report, use this to add space for manually adding documents",
            DefaultValueFactory = result => 0,
            Required = true
        };
        blankLinesOptions.Validators.Add(optionResult =>
        {
            if (optionResult.GetRequiredValue(blankLinesOptions) < 0)
            {
                optionResult.AddError("Blank lines must not be less than 0");
            }

            if (optionResult.GetRequiredValue(formatOption) != FormatType.Pdf &&
                optionResult.GetRequiredValue(blankLinesOptions) > 0)
            {
                optionResult.AddError("Blank lines can only be used with PDF format");
            }
        });

        Option<int> apiBatchSizeOption = new Option<int>("--batch-size", "-B")
        {
            Description = "Batch size for querying the paperless-ngx api",
            DefaultValueFactory = result => 50,
            Required = true
        };
        apiBatchSizeOption.Validators.Add(optionResult =>
        {
            if (optionResult.GetRequiredValue(apiBatchSizeOption) < 1)
            {
                optionResult.AddError("Batch size must be at least 1");
            }
        });

        Option<int?> asnFromOption = new Option<int?>("--asn-from", "-af")
        {
            Description = "Lower bound for ASN Selection",
        };
        asnFromOption.Validators.Add(optionResult =>
        {
            int? asnFrom = optionResult.GetValue(asnFromOption);
            if (asnFrom is not null)
            {
                if (asnFrom < 1)
                    optionResult.AddError("ASN cannot be less than 1");
            }
        });
        Option<int?> asnToOption = new Option<int?>("--asn-to", "-at")
        {
            Description = "Upper bound for ASN Selection",
        };
        asnToOption.Validators.Add(optionResult =>
        {
            int? asnTo = optionResult.GetValue(asnToOption);
            if (asnTo is not null)
            {
                if (asnTo < 1)
                    optionResult.AddError("ASN cannot be less than 1");

                int? asnFrom = optionResult.GetValue(asnFromOption);
                if (asnFrom is not null && asnTo < asnFrom.Value)
                    optionResult.AddError("ASN upper bound cannot be greater than ASN lower bound");
            }
        });

        Option<bool> ignoreBlankAsn = new Option<bool>("--ignore-blank-asn", "-i")
        {
            Description =
                "Ignore blank ASNs when querying all documents. This option has no effect when supplying an ASN range, as blank ASNs are always ignored.",
            DefaultValueFactory = result => false,
        };

        RootCommand rootCommand = new RootCommand("Create a report of all documents in you paperless-ngx instance");
        rootCommand.Add(urlOption);
        rootCommand.Add(tokenOption);
        rootCommand.Add(formatOption);
        rootCommand.Add(pathOption);
        rootCommand.Add(blankLinesOptions);
        rootCommand.Add(apiBatchSizeOption);
        rootCommand.Add(asnFromOption);
        rootCommand.Add(asnToOption);
        rootCommand.Add(ignoreBlankAsn);

        rootCommand.SetAction(result =>
        {
            string url = result.GetRequiredValue(urlOption);
            string token = result.GetRequiredValue(tokenOption);
            FormatType format = result.GetRequiredValue(formatOption);
            string pathOutput = result.GetRequiredValue(pathOption);
            int blankLines = result.GetRequiredValue(blankLinesOptions);
            int batchSize = result.GetRequiredValue(apiBatchSizeOption);
            int? asnFrom = result.GetValue(asnFromOption);
            int? asnTo = result.GetValue(asnToOption);
            bool ignoreBlank = result.GetValue(ignoreBlankAsn);


            CmdArgs arguments = new CmdArgs(url, token, format, pathOutput, blankLines, batchSize, asnFrom, asnTo,
                ignoreBlank);
            callback(arguments);
        });

        ParseResult result = rootCommand.Parse(args);

        return result.Invoke();
    }
}