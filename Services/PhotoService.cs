using conecta_api.Services.Interfaces;
using System.Text.Json;

namespace conecta_api.Services;
public class PhotoService : IPhotoService
{
    private readonly HttpClient _httpClient;
    private const string ImgBBApiUrl = "https://api.imgbb.com/1/upload";
    private const string ImgBBApiKey = "de27c44b3935fa8a497a40fd5cbddf2a";

    public PhotoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> UploadPhotos(List<string> images)
    {
        var urls = new List<string>();

        foreach (var img in images)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(img), "image");

            var response = await _httpClient.PostAsync($"{ImgBBApiUrl}?key={ImgBBApiKey}", formData);
            var resultJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception("Erro ao fazer upload da imagem");

            var result = JsonSerializer.Deserialize<JsonElement>(resultJson);
            var url = result.GetProperty("data").GetProperty("url_viewer").GetString();
            urls.Add(url);
        }

        return urls;
    }
}
