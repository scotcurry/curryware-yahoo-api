using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace curryware_yahoo_api.KafkaHandlers;

public class KafkaAdmin
{
    public List<string> GetTopics()
    {
        var bootStrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER") ??
                               "ubuntu-postgres.curryware.org:9092";
        var adminConfig = new AdminClientConfig()
        {
            BootstrapServers = bootStrapServers
        };

        using var adminClient = new AdminClientBuilder(adminConfig).Build();
        var metaData = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        var topicNames = metaData.Topics.Select(t => t.Topic).ToList();
        
        return topicNames;
    }

    public static async Task<bool> CreateTopic(string topicName)
    {
        var bootStrapServers = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER") ?? "ubuntu-postgres.curryware.org:9092";

        // This is another way to build the config to pass in.  AdminClientConfig takes a dictionary of strings.
        // var adminConfig = new Dictionary<string, string>
        // {
        //     {"bootstrap.servers", bootStrapServers}
        // };
        var adminConfigDict = new Dictionary<string, string> {{"bootstrap.servers", bootStrapServers}};
        var adminConfig = new AdminClientConfig(adminConfigDict);
        var topicCreated = false;
        
        // The AdminClientBuilder builds an iAdminClient (https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.IAdminClient.html).
        // This code is going to call CreateTopicAsync (https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.IAdminClient.html#Confluent_Kafka_IAdminClient_CreateTopicsAsync_Confluent_Kafka_TopicSpecification_System_Threading_CancellationToken_)
        // to create the topic.
        using var adminClient = new AdminClientBuilder(adminConfig).Build();
        var topicSpecification = new TopicSpecification
        {
            Name = "PlayerTopic",
            NumPartitions = 1
        };

        try
        {
            await adminClient.CreateTopicsAsync(new List<TopicSpecification> {topicSpecification});
            topicCreated = true;
        }
        catch (CreateTopicsException e)
        {
            Console.WriteLine($"Failed to create topic: {e.Error.Reason}");
        }
        
        return topicCreated;
    }
}