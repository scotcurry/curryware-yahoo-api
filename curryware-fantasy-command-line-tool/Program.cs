using curryware_kafka_command_line.CommandLineHandlers;
using curryware_kafka_command_line.CommandLineModels;
using curryware_log_handler;
using Microsoft.Extensions.Logging;

namespace curryware_kafka_command_line;

internal abstract class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var parsedCommandLine = CommandLineParser.ParseCommandLine(args);
        }
        catch (InvalidParameterException invalidParameterException)
        {
            CurrywareLogHandler.AddLog(invalidParameterException.Message, LogLevel.Error);
            PrintHelp();
            Environment.Exit(120);
        }
        var gameStatCommandInformation = new GameStatsCommandLineParameters();
        var playerCommandInformation = new PlayerCommandLineParameters();
        
        {
            PrintHelp();
            Environment.Exit(120);
        }
        
        // var bootStrapServer = "localhost:9092";
        // var consumerGroup = "curryware-group";
        // var topic = "PlayerTopic";
        //
        // // Standard stuff to set the server that will be used.
        // var kafkaConfig = new ConsumerConfig
        // {
        //     BootstrapServers = bootStrapServer,
        // };
        //
        // // A return handler for produced message.  Will write out error to message written.
        // Action<DeliveryReport<Null, string>> handler = r =>
        // {
        //     Console.WriteLine(!r.Error.IsError
        //     ? $"Delivered message to '{r.TopicPartitionOffset}': {r.Message.Value}"
        //     : $"Delivery error: {r.Error.Reason}");;
        // };
        //
        // using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
        // const string messageString = "[{\"playerID\":1,\"playerName\":\"Scot\"},{\"playerID\":2,\"playerName\":\"Otis\"}]";
        // var kafkaMessage = new Message<Null, string> { Value = messageString };
        // producer.Produce(topic, kafkaMessage, handler);
        // producer.Flush(TimeSpan.FromSeconds(2));
        //
        // ConsumeTopic(topic, consumerGroup, bootStrapServer);
    }

    
    
    private static void PrintHelp()
    {
        Console.WriteLine("Usage: curryware-fantasy-command-line-tool <stats> | <players> [options]");
        Console.WriteLine("\n");
        Console.WriteLine("Game Statistics:");
        Console.WriteLine("\tstats");
        Console.WriteLine("\t-g <gameID>");
        Console.WriteLine("\t-l <leagueID>");
        Console.WriteLine("\t [-w <week>]");
        Console.WriteLine("\t [-P <position>] [QB, RB, WR, TE, K, D] - Only one position can be provided.");
        Console.WriteLine("\n");
        Console.WriteLine("Player Information:");
        Console.WriteLine("\tplayers");
        Console.WriteLine("\t [-P <position>] [QB, RB, WR, TE, K, D] - Only one position can be provided.");
        Console.WriteLine("\t [-s <status>] [A, FA, W, T] - Only one status can be provided.");
        Console.WriteLine("\t\t A - Available");
        Console.WriteLine("\t\t FA - Free Agent");
        Console.WriteLine("\t\t W - Waivers");
        Console.WriteLine("\t\t T - Taken");
    }

    // private static void ConsumeTopic(string topic, string consumerGroup, string bootstrapServers)
    // {
    //     var kafkaConfig = new ConsumerConfig
    //     {
    //         GroupId = consumerGroup,
    //         BootstrapServers = bootstrapServers,
    //         AutoOffsetReset = AutoOffsetReset.Earliest
    //     };
    //     
    //     using var consumer = new ConsumerBuilder<Ignore, string>(kafkaConfig).Build();
    //     
    //     var cts = new CancellationTokenSource();
    //     Console.CancelKeyPress += (_, e) => {
    //         // Prevent the process from terminating.
    //         e.Cancel = true;
    //         cts.Cancel();
    //     };
    //     
    //     consumer.Subscribe(topic);
    //     try
    //     {
    //         while (true)
    //         {
    //             try
    //             {
    //                 var consumeResult = consumer.Consume(cts.Token);
    //                 if (consumeResult.IsPartitionEOF)
    //                     continue;
    //                 if (consumeResult.Message == null)
    //                     continue;
    //                 if (consumeResult.Message.Value != null) 
    //                     Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");
    //             } catch (ConsumeException e) 
    //             {
    //                 Console.WriteLine($"Error occured: {e.Error.Reason}");
    //             }
    //         }
    //     } catch (OperationCanceledException)
    //     {
    //         // Ensure the consumer leaves the group cleanly and final offsets are committed.
    //         Console.WriteLine("Closing consumer.");
    //         consumer.Close();
    //     }
    // }
}
