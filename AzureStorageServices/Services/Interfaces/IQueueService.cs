using AzureStorageServices.Models;

namespace AzureStorageServices.Services.Interfaces
{
    public interface IQueueService
    {
        Task SendMessageAsync(EmailMessage message);
    }
}