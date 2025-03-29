// using Serilog;
// using Serilog.Formatting.Json;
// using Serilog.Formatting.Compact;
//
// namespace curryware_yahoo_api.LogHandler;
//
// abstract class CurrywareLogHandler
// {
//     internal static void AddLog(string message, LogLevel level)
//     {
//         Log.Logger = new LoggerConfiguration()
//             .WriteTo.Console(new JsonFormatter())
//             .WriteTo.File(new CompactJsonFormatter(), "./logs/yahoo-api.log")
//             .CreateLogger();
//
//         switch (level)
//         {
//             case LogLevel.Debug:
//                 Log.Logger.Debug(message);
//                 break;
//             case LogLevel.Error:
//                 Log.Logger.Error(message);
//                 break;
//             case LogLevel.Information:
//                 Log.Logger.Information(message);
//                 break;
//             case LogLevel.Warning:
//                 Log.Logger.Warning(message);
//                 break;
//             case LogLevel.Trace:
//                 Log.Logger.Debug(message);
//                 break;
//             case LogLevel.None:
//                 Log.Logger.Information(message);
//                 break;
//             case LogLevel.Critical:
//             default:
//                 Log.Logger.Error(message);
//                 break;
//         }
//     }
// }