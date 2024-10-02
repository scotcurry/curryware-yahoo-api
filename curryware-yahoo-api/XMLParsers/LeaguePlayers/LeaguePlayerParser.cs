using System.Xml.Linq;
using curryware_yahoo_api.PlayerModels;

namespace curryware_yahoo_api.XMLParsers.LeagueSettingsParser.LeaguePlayers;

public class LeaguePlayerParser
{
    public static List<PlayerModel> GetParseLeaguePlayerXml(string xmlPayload)
    {
        var xmlDoc = XDocument.Parse(xmlPayload);
        XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
        var players = xmlDoc.Descendants(fantasyNameSpace + "player");
        var playersList = new List<PlayerModel>();

        var totalPlayersNode = xmlDoc.Descendants(fantasyNameSpace + "league");
        var availablePlayers = totalPlayersNode.Any();

        if (availablePlayers)
        {
            foreach (var currentPlayer in players)
            {
                const int defenseIds = 100000;
                string playerKey = String.Empty;
                int playerId = 0;
                string fullName = String.Empty;
                string url = String.Empty;
                string status = String.Empty;
                string team = String.Empty;
                int byeWeek = 0;
                int uniformNumber = 0;
                string headShot = String.Empty;
                string primaryPosition = String.Empty;

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
                    if (byeWeekElement!.Element(fantasyNameSpace + "bye_week") != null)
                    {
                        byeWeek = int.Parse(byeWeekElement.Element(fantasyNameSpace + "bye_week")!.Value);
                    }
                }

                if (currentPlayer.Element(fantasyNameSpace + "uniform_number") != null && playerId < defenseIds)
                {
                    var uniformNumberString = currentPlayer.Element(fantasyNameSpace + "uniform_number")!.Value;
                    if (uniformNumberString != String.Empty)
                    {
                        uniformNumber = int.Parse(currentPlayer.Element(fantasyNameSpace + "uniform_number")!.Value);
                    }
                    else
                    {
                        uniformNumber = 0;
                    }
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

        return playersList;
    }
}