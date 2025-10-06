namespace PaperMore.Reports;

internal interface IReportGenerator
{
    void Generate(List<DocumentReportData> data, Stream outputStream);
}