using conecta_api.Services.Interfaces;
using System.Text.Json;

namespace conecta_api.Services;
public class PhotoService : IPhotoService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;


    public PhotoService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<string>> UploadPhotos(List<string> images)
    {
        var ImgBBApiUrl = _configuration["ImgBB:UrlApi"];
        var apiKey = _configuration["ImgBB:ApiKey"];

        var urls = new List<string>();

        foreach (var img in images)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(img), "image");

            var response = await _httpClient.PostAsync($"{ImgBBApiUrl}?key={apiKey}", formData);
            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao fazer upload da imagem");

            var result = JsonSerializer.Deserialize<JsonElement>(resultJson);
            var url = result.GetProperty("data").GetProperty("url").GetString();
            urls.Add(url);
        }

        return urls;
    }
}
