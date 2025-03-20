using Azure.Storage.Queues;
using AzureStorageServices.Models;
using AzureStorageServices.Options;
using AzureStorageServices.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AzureStorageServices.Services
{
    public class QueueService : IQueueService
    {
        private readonly IOptions<AzureQueueStorageOptions> azureQueueStorageOptions;

        public QueueService(IOptions<AzureQueueStorageOptions> azureQueueStorageOptions)
        {
            this.azureQueueStorageOptions = azureQueueStorageOptions;
        }

        public async Task SendMessageAsync(EmailMessage message)
        {
            QueueClient client = await GetQueueClientAsync();
            await client.SendMessageAsync(JsonConvert.SerializeObject(message));
        }

        private async Task<QueueClient> GetQueueClientAsync()
        {
            QueueClient client = new QueueClient(azureQueueStorageOptions.Value.ConnectionString, azureQueueStorageOptions.Value.QueueName);
            // Create the queue if it doesn't already exist
            await client.CreateIfNotExistsAsync();
            return client ?? throw new ArgumentNullException(nameof(QueueClient));
        }
    }
}
