using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using curryware_data_models;
using Serilog;

namespace curryware_yahoo_parsing_library.XmlParsers.StatsParsers;

internal abstract class WeeklyStatParser
{
    internal static string WeeklyStats(string xmlPayload, string oauthToken, int gameId)
    {
        var playerStats = new List<PlayerStats>();
        try
        {
            var xmlDoc = XDocument.Parse(xmlPayload);
            XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
            var players = xmlDoc.Descendants(fantasyNameSpace + "player");
            foreach (var currentPlayer in players)
            {
                var playerIdNode = currentPlayer.Element(fantasyNameSpace + "player_id");
                var playerId = 0;
                var week = 0;
                var statId = 0;
                var statValue = 0;
                
                if (playerIdNode != null)
                {
                    playerId = Convert.ToInt32(playerIdNode.Value);
                }
                
                var playerStatsNode = currentPlayer.Element(fantasyNameSpace + "player_stats");
                var weekNode = playerStatsNode?.Element(fantasyNameSpace + "week");
                if (weekNode != null)
                    week = Convert.ToInt32(weekNode.Value);

                var statsNodes = currentPlayer.Descendants(fantasyNameSpace + "stat");
                foreach (var currentStatNode in statsNodes)
                {
                    var statIdNode = currentStatNode.Element(fantasyNameSpace + "stat_id");
                    if (statIdNode != null)
                    {
                        statId = Convert.ToInt32(statIdNode.Value);
                    }

                    var statValueNode = currentStatNode.Element(fantasyNameSpace + "value");

                    if (statValueNode != null)
                    {
                        statValue = Convert.ToInt32(statValueNode.Value);
                    }
                    
                    var playerStat = new PlayerStats
                    {
                        PlayerId = playerId,
                        GameKey = gameId,
                        WeekKey = week,
                        StatId = statId,
                        StatValue = statValue
                    };
                    playerStats.Add(playerStat);
                }
            }
        }
        catch (XmlException xmlException)
        {
            // CurrywareLogHandler.AddLog(xmlException.Message, LogLevel.Error);
            Log.Error(xmlException, "Error: Failed to parse league information");
            // CurrywareLogHandler.AddLog("Error: Failed to parse league information", LogLevel.Error);
            return "Error: Failed to parse league information";
        }
        catch (FormatException formatException)
        {
            // CurrywareLogHandler.AddLog(formatException.Message, LogLevel.Error);
            Log.Error(formatException, "Error: Failed to parse league information");
            // CurrywareLogHandler.AddLog("Error: Failed to parse league information", LogLevel.Error);
            return "Error: Failed to parse league information";
        }

        try
        {
                var serializerOptions = new JsonSerializerOptions
                {
                    WriteIndented = false
                };
                var playersWithMetaData = new PlayerStatsWithMetadata
                {
                    OAuthToken = oauthToken,
                    PlayerStats = playerStats
                };
                var json = JsonSerializer.Serialize(playersWithMetaData, serializerOptions);
                return json;
        }
        catch (ArgumentNullException argumentNullException)
        {
            // CurrywareLogHandler.AddLog(argumentNullException.Message, LogLevel.Error);
            Log.Error(argumentNullException, "Error: Failed to serialize league stat settings");
            // CurrywareLogHandler.AddLog("Error: Failed to serialize league stat settings", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
        catch (InvalidCastException invalidCastException)
        {
            // CurrywareLogHandler.AddLog(invalidCastException.Message, LogLevel.Error);
            Log.Error(invalidCastException, "Error: Failed to serialize league stat settings");
            // CurrywareLogHandler.AddLog("Error: Failed to serialize league stat settings", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
    }
}