using System.Globalization;

namespace PaperMore.Reports;

public record ExportFormatOptions(string DateFormat)
{
    public static ExportFormatOptions Default =>
        new ExportFormatOptions(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
}