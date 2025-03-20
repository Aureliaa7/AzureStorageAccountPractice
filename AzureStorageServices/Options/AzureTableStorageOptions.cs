namespace AzureTables.Options
{
    public class AzureTableStorageOptions
    {
        public const string AzureTableStorage = "AzureTableStorage";

        public string StorageUri { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        public string StorageAccountName { get; set; } = string.Empty;

        public string StorageAccountKey { get; set; } = string.Empty;
    }
}
