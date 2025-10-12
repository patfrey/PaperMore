using PaperMore.Reports;

namespace PaperMore.Api;

public interface IDocumentDataSource
{
    List<DocumentReportData> GetDocumentData(DocumentQueryParams queryParams);
}