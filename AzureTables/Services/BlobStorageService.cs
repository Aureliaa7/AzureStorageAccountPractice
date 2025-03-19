using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using AzureTables.Options;
using AzureTables.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureTables.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IOptions<AzureContainerOptions> azureStorageOptions;

        public BlobStorageService(IOptions<AzureContainerOptions> azureStorageOptions)
        {
            this.azureStorageOptions = azureStorageOptions;
        }

        public async Task<string> UploadAsync(IFormFile formFile, string imageName)
        {
            string blobName = $"{imageName}{Path.GetExtension(formFile.FileName)}";
            BlobContainerClient containerClient = await GetBlobContainerClientAsync();

            using var ms = new MemoryStream();
            formFile.CopyTo(ms);
            ms.Position = 0;
            BlobClient blob = containerClient.GetBlobClient(imageName);
            await blob.UploadAsync(ms, overwrite: true);

            return blobName;
        }

        public async Task<string> GetUrlAsync(string imageName)
        {
            BlobContainerClient containerClient = await GetBlobContainerClientAsync();
            BlobClient blob = containerClient.GetBlobClient(imageName);

            BlobSasBuilder blobSasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blob.BlobContainerName,
                BlobName = blob.Name,
                ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                Protocol = SasProtocol.Https,
                Resource = "b" // the shared resource is a blob
            };

            blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            return blob.GenerateSasUri(blobSasBuilder).ToString();
        }

        public async Task RemoveAsync(string imageName)
        {
            BlobContainerClient container = await GetBlobContainerClientAsync();
            BlobClient blob = container.GetBlobClient(imageName);
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

        private async Task<BlobContainerClient> GetBlobContainerClientAsync()
        {
            BlobContainerClient client = new BlobContainerClient(azureStorageOptions.Value.ConnectionString,
            azureStorageOptions.Value.ContainerName);
            // Create the container if it doesn't already exist
            await client.CreateIfNotExistsAsync();
            return client ?? throw new ArgumentNullException(nameof(BlobContainerClient));
        }
    }
}
