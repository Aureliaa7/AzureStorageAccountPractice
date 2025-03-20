using Azure;
using Azure.Data.Tables;
using AzureTables.Data;
using AzureTables.Options;
using AzureTables.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace AzureTables.Services
{
    public class UserService : IUserService
    {
        private readonly IOptions<AzureTableStorageOptions> azureTableStorageOptions;
        public UserService(IOptions<AzureTableStorageOptions> azureTableStorageOptions)
        {
            this.azureTableStorageOptions = azureTableStorageOptions;
        }

        public async Task DeleteAsync(string country, string id)
        {
            var client = await GetTableClientAsync();
            await client.DeleteEntityAsync(country, id);
        }

        public async Task<List<UserEntity>> GetAllAsync()
        {
            var client = await GetTableClientAsync();
            Pageable<UserEntity> usersEntities = client.Query<UserEntity>();
            return usersEntities.ToList();
        }

        public async Task<UserEntity> GetAsync(string country, string id)
        {
            var client = await GetTableClientAsync();
            var response = await client.GetEntityAsync<UserEntity>(partitionKey: country, id);

            return response.Value;
        }

        public async Task UpsertAsync(UserEntity user)
        {
            var client = await GetTableClientAsync();
            await client.UpsertEntityAsync(user);
        }

        private async Task<TableClient> GetTableClientAsync()
        {
            var client = new TableClient(new Uri(azureTableStorageOptions.Value.StorageUri),
                 azureTableStorageOptions.Value.TableName,
        new TableSharedKeyCredential(azureTableStorageOptions.Value.StorageAccountName, azureTableStorageOptions.Value.StorageAccountKey));

            // Create the table if it doesn't already exist
            await client.CreateIfNotExistsAsync();
            return client ?? throw new ArgumentNullException(nameof(TableClient));
        }
    }
}
