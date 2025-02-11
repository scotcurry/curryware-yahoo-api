using System.Xml;
using System.Text.Json;
using System.Xml.Linq;
using curryware_yahoo_parsing_library.LogHandler;
using curryware_data_models;
using Microsoft.Extensions.Logging;

namespace curryware_yahoo_parsing_library.XmlParsers.LeaguePlayers;

internal abstract class LeaguePlayerParser
{
    internal static string GetParseLeaguePlayerXml(string xmlPayload, string oauthToken)
    {
        var playersList = new List<PlayerModel>();
        var playersOnPage = 0;
        
        try
        {
            var xmlDoc = XDocument.Parse(xmlPayload);
            XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
            var players = xmlDoc.Descendants(fantasyNameSpace + "player");

            var totalPlayersNode = xmlDoc.Descendants(fantasyNameSpace + "players");
            var playersNode = totalPlayersNode as XElement[] ?? totalPlayersNode.ToArray();
            var firstNode = playersNode.First();
            if (firstNode.HasAttributes)
            {
                playersOnPage = int.Parse(firstNode.Attribute("count")!.Value);
            }

            var availablePlayers = playersNode.Any();
            if (!availablePlayers) return null;

            foreach (var currentPlayer in players)
            {
                const int defenseIds = 100000;
                string playerKey = string.Empty;
                int playerId = 0;
                string fullName = string.Empty;
                string url = string.Empty;
                string status = string.Empty;
                string team = string.Empty;
                int byeWeek = 0;
                int uniformNumber = 0;
                string headShot = string.Empty;
                string primaryPosition = string.Empty;

                // var node = currentPlayer.DescendantNodes();
                if (currentPlayer.Element(fantasyNameSpace + "player_id") != null)
                {
                    playerId = int.Parse(currentPlayer.Element(fantasyNameSpace + "player_id")!.Value);
                }

                if (currentPlayer.Element(fantasyNameSpace + "player_key") != null)
                {
                    playerKey = currentPlayer.Element(fantasyNameSpace + "player_key")!.Value;
                }

                if (currentPlayer.Element(fantasyNameSpace + "name") != null)
                {
                    var nameElement = currentPlayer.Element(fantasyNameSpace + "name");
                    if (nameElement!.Element(fantasyNameSpace + "full") != null)
                    {
                        fullName = nameElement.Element(fantasyNameSpace + "full")!.Value;
                    }
                }

                if (currentPlayer.Element(fantasyNameSpace + "url") != null)
                {
                    url = currentPlayer.Element(fantasyNameSpace + "url")!.Value;
                }

                if (currentPlayer.Element(fantasyNameSpace + "status") != null && playerId < defenseIds)
                {
                    status = currentPlayer.Element(fantasyNameSpace + "status")!.Value;
                }

                if (currentPlayer.Element(fantasyNameSpace + "editorial_team_abbr") != null)
                {
                    team = currentPlayer.Element(fantasyNameSpace + "editorial_team_abbr")!.Value;
                }

                if (currentPlayer.Element(fantasyNameSpace + "bye_weeks") != null)
                {
                    var byeWeekElement = currentPlayer.Element(fantasyNameSpace + "bye_weeks");
                    if (byeWeekElement!.Element(fantasyNameSpace + "week") != null)
                    {
                        byeWeek = int.Parse(byeWeekElement.Element(fantasyNameSpace + "week")!.Value);
                    }
                }

                if (currentPlayer.Element(fantasyNameSpace + "uniform_number") != null && playerId < defenseIds)
                {
                    var uniformNumberString = currentPlayer.Element(fantasyNameSpace + "uniform_number")!.Value;
                    uniformNumber = uniformNumberString != String.Empty ? int.Parse(currentPlayer.Element(fantasyNameSpace + "uniform_number")!.Value) : 0;
                }

                if (currentPlayer.Element(fantasyNameSpace + "primary_position") != null)
                {
                    primaryPosition = currentPlayer.Element(fantasyNameSpace + "primary_position")!.Value;
                }

                if (currentPlayer.Element(fantasyNameSpace + "headshot") != null)
                {
                    var headShotElement = currentPlayer.Element(fantasyNameSpace + "headshot");
                    if (headShotElement!.Element(fantasyNameSpace + "url") != null)
                    {
                        headShot = headShotElement.Element(fantasyNameSpace + "url")!.Value;
                    }
                }

                var player = new PlayerModel
                {
                    Key = playerKey,
                    Id = playerId,
                    FullName = fullName,
                    Url = url,
                    Status = status,
                    Team = team,
                    ByeWeek = byeWeek,
                    UniformNumber = uniformNumber,
                    Position = primaryPosition,
                    Headshot = headShot
                };

                playersList.Add(player);
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

            var playersWithCount = new PlayersListWithCount
            {
                Players = playersList,
                PlayerCount = playersOnPage,
                OAuthToken = oauthToken,
            };

            var json = JsonSerializer.Serialize(playersWithCount, serializerOptions);
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
}