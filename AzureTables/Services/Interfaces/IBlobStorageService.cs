namespace AzureTables.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> GetUrlAsync(string imageName);

        Task RemoveAsync(string imageName);

        Task<string> UploadAsync(IFormFile formFile, string imageName);
    }
}