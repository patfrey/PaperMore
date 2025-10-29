namespace PaperMore.Reports;

internal interface IReportGenerator
{
    void Generate(List<DocumentReportData> data, Comparison<DocumentReportData> sorting,
        Func<DocumentReportData, bool> filter, Stream outputStream);
}