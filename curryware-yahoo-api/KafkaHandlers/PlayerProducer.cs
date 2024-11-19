using Confluent.Kafka;
using Serilog;
using Serilog.Formatting.Json;

namespace curryware_yahoo_api.KafkaHandlers;

public class PlayerProducer
{
     public static async Task<bool> SendPlayerData(string topic, string value)
     {
         Log.Logger = new LoggerConfiguration()
             .WriteTo.Console(new JsonFormatter())
             .CreateLogger();
         
         var bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
         if (bootStrapServer == null)
         {
             bootStrapServer = "localhost:9092";
         }
         
         Log.Information("Kafka Bootstrap Server: {bootstrapServer}", bootStrapServer);
         // All the values required for this class are documented at (need to research requirements):
         // https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ProducerConfig.html
         var config = new ProducerConfig
         {
             BootstrapServers = bootStrapServer,
             ClientId = "SendPlayerData"
         };

         Guid guid = new Guid();
         var key = "key_" + guid;
         var message = new Message<string, string>
         {
             Key = key,
             Value = value
         };
         
         var topicExists = GetValidateTopicExists(topic);
         if (!topicExists)
         {
             Log.Error("Topic does not exist");
             return false;
         }
         
         var producer = new ProducerBuilder<string, string>(config).Build();
         var deliveryReport = await producer.ProduceAsync(topic, message);
         Log.Information($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'");
         
         producer.Flush(TimeSpan.FromSeconds(10));
         Log.Information("Flushing");
         return true;
     }

     private static bool GetValidateTopicExists(string topicName)
     {
         var kafkaAdmin = new KafkaAdmin();
         var allTopics = kafkaAdmin.GetTopics();
         var topicExists = false;
         for (int i = 0; i < allTopics.Count; i++)
         {
             var currentTopic = allTopics[i];
             if (currentTopic != topicName) continue;
             topicExists = true;
             i = allTopics.Count;
         }
         return topicExists;
     }
}