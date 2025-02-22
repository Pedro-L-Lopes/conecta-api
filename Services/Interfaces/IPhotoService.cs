namespace conecta_api.Services.Interfaces;
public interface IPhotoService
{
    Task<List<string>> UploadPhotos(List<string> images);
}
