namespace PaperMore.Reports;

public abstract class GeneratorBase : IReportGenerator
{
    public void Generate(List<DocumentReportData> data, Comparison<DocumentReportData> sorting,
        Func<DocumentReportData, bool> filter, Stream outputStream)
    {
        List<DocumentReportData> preparedData = data.Where(filter)
            .ToList();

        preparedData.Sort(sorting);

        Generate(preparedData, outputStream);
    }

    protected abstract void Generate(List<DocumentReportData> data, Stream outputStream);
}