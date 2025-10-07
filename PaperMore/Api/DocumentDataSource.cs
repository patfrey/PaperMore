using CommunityToolkit.Diagnostics;
using Paperless.Client.Gen;
using PaperMore.Reports;

namespace PaperMore.Api;

internal class DocumentDataSource
{
    const int PageSize = 50;
    public List<DocumentReportData> GetDocumentData(string apiEndpoint, string token)
    {
        using HttpClient client = new HttpClient();
        PaperlessApiClient paperless = new PaperlessApiClient(apiEndpoint, token, client);
        Task<List<DocumentReportData>> dataTask = QueryDocumentsAsync(paperless);
        dataTask.Wait();

        List<DocumentReportData> data = dataTask.Result.ToList();
        
        return data;
    }

    private async Task<List<DocumentReportData>> QueryDocumentsAsync(PaperlessApiClient paperless)
    {
        List<DocumentReportData> results = new List<DocumentReportData>();
        List<Document> documents = new List<Document>();

        PaginatedDocumentList? page;
        int currentPage = 1;
        do
        {
            page = await paperless.ApiDocumentsGetAsync(page: currentPage, page_size: PageSize);
            foreach (Document document in page.Results)
            {
                documents.Add(document);    
            }

            currentPage++;
        } while (page?.Next is not null);
        
        
        List<Correspondent> correspondents = await GetCorrespondents(paperless, documents);

        foreach(Document doc in documents)
        {
            // If a name is null we have messed up somewhere else
            Guard.IsNotNullOrEmpty(doc.Title, nameof(doc.Title));
            
            string correspondent = string.Empty;
            if (doc.Correspondent is not null)
            {
                string? name = correspondents
                    .Where(c => c.Id == doc.Correspondent)
                    .Select(c => c.Name)
                    .FirstOrDefault();
                // We specifically selected a correspondent based on this documents id, this better not be null
                Guard.IsNotNull(name, nameof(name));
                
                correspondent = name;
            }
            
            DocumentReportData data = new DocumentReportData(doc.Title!, doc.Archive_serial_number, correspondent, doc.Created ?? DateTimeOffset.Now, doc.Added);
            
            results.Add(data);
        }
        
        return results;
    }

    private async Task<List<Correspondent>> GetCorrespondents(PaperlessApiClient paperless, List<Document> documents)
    {
        List<Correspondent> results = new List<Correspondent>();
        
        List<int> correspondentsIds = documents.Select(doc => doc.Correspondent)
            .Where(id => id is not null)
            .Select(id => id!.Value)
            .Distinct()
            .ToList();

        PaginatedCorrespondentList? correspondents;
        int currentPage = 1;
        do
        {
            correspondents = await paperless.ApiCorrespondentsGetAsync(id__in: correspondentsIds, page: currentPage, page_size: PageSize);
            foreach (Correspondent correspondent in correspondents.Results)
            {
                results.Add(correspondent);
            }

            currentPage++;
        } while (correspondents?.Next is not null);
        
        return results;
    }
}