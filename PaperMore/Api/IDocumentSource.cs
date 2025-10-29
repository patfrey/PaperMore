using PaperMore.Reports;

namespace PaperMore.Api;

public interface IDocumentSource
{
    List<DocumentReportData> GetDocumentData(DocumentQueryParams queryParams);
}