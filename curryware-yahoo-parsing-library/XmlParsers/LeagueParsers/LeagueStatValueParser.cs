using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

using curryware_yahoo_parsing_library.LeagueModels;
using curryware_yahoo_parsing_library.LogHandler;

namespace curryware_yahoo_parsing_library.XmlParsers.LeagueParsers;

internal class LeagueStatValueParser
{
    internal static string GetLeagueStatValuesFromXml(string xmlPayload, int gameId, int leagueId)
    {
        var leagueStatValues = new List<LeagueStatsValueModel>();

        try
        {
            var xmlDoc = XDocument.Parse(xmlPayload);
            XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";

            // This is used to create a key for the table it takes the league key without the .l. and 
            // adds the stat id and creates a BIGINT.  This gets calculated before creating the JSON
            var leagueKeyNode = xmlDoc.Root?.Descendants(fantasyNameSpace + "league_key");
            var leagueKeyString = leagueKeyNode?.FirstOrDefault()?.Value;
            if (leagueKeyString == null)
            {
                return "Error: Failed to parse league information";
            }

            var statsCategoriesNode = xmlDoc.Root?.Descendants(fantasyNameSpace + "stat_modifiers");
            var allStatsInCategory = statsCategoriesNode?.Descendants(fantasyNameSpace + "stat");
            if (allStatsInCategory != null)
            {
                leagueStatValues.AddRange(allStatsInCategory.Select(currentStatsNode => ParseCurrentStatsNode(currentStatsNode, gameId, leagueId)));
            }
        }
        catch (XmlException xmlException)
        {
            CurrywareLogHandler.AddLog(xmlException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to parse league information", LogLevel.Error);
            return "Error: Failed to parse league information";
        }
        catch (FormatException formatException)
        {
            CurrywareLogHandler.AddLog(formatException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to parse league information", LogLevel.Error);
            return "Error: Failed to parse league information";
        }
        
        try
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
            };

            var json = JsonSerializer.Serialize(leagueStatValues, serializerOptions);
            return json;
        }
        catch (ArgumentNullException argumentNullException)
        {
            CurrywareLogHandler.AddLog(argumentNullException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to serialize league stat settings", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
        catch (InvalidCastException invalidCastException)
        {
            CurrywareLogHandler.AddLog(invalidCastException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to serialize league stat settings", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
    }
    
    private static LeagueStatsValueModel ParseCurrentStatsNode(XElement currentStatsNode, int gameId, int leagueId)
    {
        var statId = 0;
        decimal statValue = 0;

        foreach (var currentNode in currentStatsNode.Descendants())
        {
            if (currentNode.Name.LocalName == "stat_id")
                statId = Convert.ToInt32(currentNode.Value);
            if (currentNode.Name.LocalName == "value")
            {
                statValue = Convert.ToDecimal(currentNode.Value);
            }
        }

        var leagueStat = new LeagueStatsValueModel
        {
            GameId = gameId,
            LeagueId = leagueId,
            StatId = statId,
            StatValue = statValue
        };
        
        return leagueStat;
    }
    
    private static Int64 BuildStatKey(string key, int statId)
    {
        var statKeyParts = key.Split('.');
        var game = statKeyParts[0];
        var league = statKeyParts[2];
        var statIdString = Convert.ToString(statId);
        
        return Convert.ToInt64(game + league + statIdString);
    }
}