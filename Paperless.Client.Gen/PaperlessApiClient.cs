using System.Net.Http.Headers;

namespace Paperless.Client.Gen;

public partial class PaperlessApiClient
{
    public string? Token
    {
        get => _httpClient.DefaultRequestHeaders.Authorization?.Parameter;
        set => _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", value);
    }

    public PaperlessApiClient(string baseUrl, string token, HttpClient client) : this(baseUrl, client)
    {
        Token = token;
    }
}