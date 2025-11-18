namespace PaperMore.CLI;

public record CmdArgs(
    string Url,
    string Token,
    FormatType Format,
    string OutputPath,
    int BlankLines,
    int BatchSize,
    int? AsnRangeFrom,
    int? AsnRangeTo,
    bool IgnoreBlankAsn);