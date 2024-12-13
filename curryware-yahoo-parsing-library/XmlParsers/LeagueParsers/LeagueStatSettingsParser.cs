using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using curryware_yahoo_parsing_library.LeagueModels;
using Microsoft.Extensions.Logging;

using curryware_yahoo_parsing_library.LogHandler;

namespace curryware_yahoo_parsing_library.XmlParsers.LeagueParsers;

internal class LeagueStatSettingsParser
{
    internal static string GetLeagueStatSettingsFromXml(string xmlPayload)
    {
        var leagueStatCategories = new List<LeagueStatsCategoryModel>();
        
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
            
            var statsCategoriesNode = xmlDoc.Root?.Descendants(fantasyNameSpace + "stat_categories");
            var allStatsInCategory = statsCategoriesNode?.Descendants(fantasyNameSpace + "stat");
            if (allStatsInCategory != null)
            {
                foreach (var currentStatsNode in allStatsInCategory)
                {
                    var leagueStatCategory = ParseCurrentStatsNode(currentStatsNode);
                    var statKey = BuildStatKey(key: leagueKeyString, statId: leagueStatCategory.StatId);
                    leagueStatCategory.LeagueStatKey = statKey;
                    leagueStatCategories.Add(leagueStatCategory);
                }
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

            var json = JsonSerializer.Serialize(leagueStatCategories, serializerOptions);
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

    private static LeagueStatsCategoryModel ParseCurrentStatsNode(XElement currentStatsNode)
    {
        var statId = 0;
        var statEnabled = false;
        var statName = "Unknown";
        var statDisplayName = "Unknown";
        var statGroup = "Unknown";
        var statAbbreviation = "Unknown";
        var statSortOrder = 0;
        var statSortPosition = 0;
        var statPositionType = "Unknown";
        
        foreach (var currentNode in currentStatsNode.Descendants())
        {
            if (currentNode.Name.LocalName == "stat_id")
                statId = Convert.ToInt32(currentNode.Value);
            if (currentNode.Name.LocalName == "enabled")
            {
                var enabledString = currentNode.Value;
                if (enabledString == "1")
                    statEnabled = true;
            }
            if (currentNode.Name.LocalName == "name")
                statName = currentNode.Value;
            if (currentNode.Name.LocalName == "display_name")
                statDisplayName = currentNode.Value;
            if (currentNode.Name.LocalName == "group")
                statGroup = currentNode.Value;
            if (currentNode.Name.LocalName == "abbr")
                statAbbreviation = currentNode.Value;
            if (currentNode.Name.LocalName == "sort_order")
                statSortOrder = Convert.ToInt32(currentNode.Value);
            if (currentNode.Name.LocalName == "position_type")
                statPositionType = currentNode.Value;
            if (currentNode.Name.LocalName == "sort_position")
                statSortPosition = Convert.ToInt32(currentNode.Value);
        }
        
        var leagueStatCategory = new LeagueStatsCategoryModel
        {
            StatId = statId,
            StatEnabled = statEnabled,
            StatName = statName,
            StatDisplayName = statDisplayName,
            StatGroup = statGroup,
            StatAbbreviation = statAbbreviation,
            StatSortOrder = statSortOrder,
            StatPositionType = statPositionType,
            StatSortPostion = statSortPosition
        };

        return leagueStatCategory;
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