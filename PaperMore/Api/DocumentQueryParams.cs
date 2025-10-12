namespace PaperMore.Api;

public class DocumentQueryParams
{
    public string ApiEndpoint { get; set; }
    public string Token { get; set; } 
    public int PageSize { get; set; }

    public DocumentQueryParams(string apiEndpoint, string token, int pageSize)
    {
        ApiEndpoint = apiEndpoint;
        Token = token;
        PageSize = pageSize;
    }
}