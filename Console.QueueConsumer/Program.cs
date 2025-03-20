using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>();

IConfigurationRoot configuration = builder.Build();

string? connectionString = configuration["AzureStorageAccountConnection"];
string? queueName = configuration["AzureQueueName"];

QueueClient queue = new QueueClient(connectionString, queueName);
if (await queue.ExistsAsync())
{
    await ProcessMessagesAsync();
}

async Task ProcessMessagesAsync()
{
    QueueMessage[] messages = await queue.ReceiveMessagesAsync(maxMessages: 10);

    foreach (QueueMessage message in messages)
    {
        Console.WriteLine($"Message: {message.MessageText}");
    }
}