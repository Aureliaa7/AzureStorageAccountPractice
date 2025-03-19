namespace AzureTables.Options
{
    public class AzureContainerOptions
    {
        public const string AzureContainerStorage = "AzureContainerStorage";

        public string ConnectionString { get; set; } = string.Empty;

        public string ContainerName { get; set; } = string.Empty;
    }
}
