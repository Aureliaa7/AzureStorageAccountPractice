namespace AzureStorageServices.Options
{
    public class AzureQueueStorageOptions
    {
        public const string AzureQueueStorage = "AzureQueueStorage";


        public string ConnectionString { get; set; } = string.Empty;

        public string QueueName { get; set; } = string.Empty;
    }
}
