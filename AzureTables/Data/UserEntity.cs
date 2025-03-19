using Azure;
using Azure.Data.Tables;

namespace AzureTables.Data
{
    public class UserEntity : ITableEntity
    {
        // Custom properties
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }


        // Added by default when implementing ITableEntity
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
