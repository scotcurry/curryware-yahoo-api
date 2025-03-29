using System.Net.Sockets;
using Serilog;

namespace curryware_kafka_producer_library;

public abstract class ValidateKafkaSettings
{
     public static bool ValidateSettings()
    {
        // CurrywareLogHandler.AddLog("Validating Kafka Port", LogLevel.Information);
        Log.Information("Validating Kafka Port");
        
        var bootStrapServer = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP_SERVER");
        if (bootStrapServer == null)
            throw new KafkaValidationException("No Kafka Bootstrap Server Environment Variable Set!");
        
        var partsToValidate = bootStrapServer.Split(":");
        var host = partsToValidate[0];
        var portString = partsToValidate[1];
        var port = Convert.ToInt32(portString);
        // CurrywareLogHandler.AddLog($"Kafka Host: {host}, Kafka Port: {port}", LogLevel.Information);
        Log.Debug($"Kafka Host: {host}, Kafka Port: {port}");

        try
        {
            using var client = new TcpClient();
            var testResult = client.BeginConnect(host, port, null, null);
            var success = testResult.AsyncWaitHandle.WaitOne(1000);

            if (!success)
            {
                // CurrywareLogHandler.AddLog("Kafka Port not open", LogLevel.Error);
                Log.Error("Kafka Port not open");
                throw new KafkaValidationException("Kafka Port Not Listening");
            }

            client.EndConnect(testResult);
            return true;
        }
        catch (ArgumentNullException argumentNull)
        {
            // CurrywareLogHandler.AddLog(argumentNull.Message, LogLevel.Error);
            Log.Error(argumentNull.Message);
            throw new KafkaValidationException("Argument null!", argumentNull);
        }
        catch (SocketException socketException)
        {
            // CurrywareLogHandler.AddLog(socketException.Message, LogLevel.Error);
            Log.Error(socketException.Message);
            throw new KafkaValidationException("Socket Exception!", socketException);
        }
        catch (ArgumentException argumentException)
        {
            // CurrywareLogHandler.AddLog(argumentException.Message, LogLevel.Error);
            Log.Error(argumentException.Message);
            throw new KafkaValidationException("Argument Exception!", argumentException);
        }
        catch (PlatformNotSupportedException platformNotSupportedException)
        {
            // CurrywareLogHandler.AddLog(platformNotSupportedException.Message, LogLevel.Error);
            Log.Error(platformNotSupportedException.Message);
            throw new KafkaValidationException("Platform Not Supported!", platformNotSupportedException);
        }
    }
    
    public static bool GetValidateTopicExists(string topicName)
    {
        Log.Debug("Validating Kafka Topic");
        var allTopics = KafkaAdmin.GetTopics();
        var topicExists = false;
        for (var i = 0; i < allTopics.Count; i++)
        {
            var currentTopic = allTopics[i];
            if (currentTopic == topicName)
            {
                topicExists = true;
                i = allTopics.Count;
                //CurrywareLogHandler.AddLog("Kafka Topic Exists", LogLevel.Information);
                Log.Debug($"Kafka topic exists: {topicName}");
            }
            else
            {
                //CurrywareLogHandler.AddLog("Kafka Topic Doesn't Exists", LogLevel.Error);
                Log.Debug($"Kafka topic does not exist: {topicName}");
            }
        } 
        
        return topicExists;
    }
}