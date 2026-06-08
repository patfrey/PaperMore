namespace PaperMore.Reports;

public abstract class GeneratorBase : IReportGenerator
{
    protected ExportFormatOptions FormatOptions
    {
        get => field ?? ExportFormatOptions.Default;
        private set;
    }

    public void Generate(List<DocumentReportData> data, Comparison<DocumentReportData> sorting,
        Func<DocumentReportData, bool> filter, ExportFormatOptions formatOptions, Stream outputStream)
    {
        List<DocumentReportData> preparedData = data.Where(filter)
            .ToList();

        FormatOptions = formatOptions;
        preparedData.Sort(sorting);

        Generate(preparedData, outputStream);
    }

    protected abstract void Generate(List<DocumentReportData> data, Stream outputStream);
}