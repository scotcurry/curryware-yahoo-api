// using Confluent.Kafka;
// using curryware_yahoo_api.LogHandler;
//
// namespace curryware_yahoo_api.KafkaHandlers;
//
// public static class PlayerProducer
// {
//      public static async Task<bool> SendPlayerData(string topic, string value)
//      {
//         // Check to see if the environment variable even exists.
//          var bootStrapServer = string.Empty;
//          try
//          {
//              if (ValidateKafkaSettings.ValidateSettings())
//                 bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
//              
//             CurrywareLogHandler.AddLog("Kafka Bootstrap Server: " + bootStrapServer, LogLevel.Information);
//              // All the values required for this class are documented at (need to research requirements):
//              // https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.ProducerConfig.html
//              var config = new ProducerConfig
//              {
//                  BootstrapServers = bootStrapServer,
//                  ClientId = "SendPlayerData"
//              };
//
//              Guid guid = Guid.NewGuid();
//              var key = "key_" + guid;
//              var message = new Message<string, string>
//              {
//                  Key = key,
//                  Value = value
//              };
//          
//              var topicExists = ValidateKafkaSettings.GetValidateTopicExists(topic);
//              if (!topicExists)
//              {
//                  CurrywareLogHandler.AddLog("Topic does not exist, building topic " + topic, LogLevel.Information);
//                  var createResult = await KafkaAdmin.CreateTopic(topic);
//                  CurrywareLogHandler.AddLog("Create Result: " + createResult, LogLevel.Information);
//                  if (createResult == false)
//                  {
//                      CurrywareLogHandler.AddLog("Failed to create topic", LogLevel.Error);
//                      return false;
//                  }
//              }
//          
//              var producer = new ProducerBuilder<string, string>(config).Build();
//              var deliveryReport = await producer.ProduceAsync(topic, message);
//              CurrywareLogHandler.AddLog($"Delivered '{deliveryReport.Value}' to '{deliveryReport.TopicPartitionOffset}'", LogLevel.Information);
//          
//              producer.Flush(TimeSpan.FromSeconds(10));
//              CurrywareLogHandler.AddLog("Flushing", LogLevel.Information);
//              return true;
//          }
//          catch (KafkaValidationException kafkaException)
//          {
//              CurrywareLogHandler.AddLog(kafkaException.Message, LogLevel.Error);
//              throw;
//          }
//      }
// }