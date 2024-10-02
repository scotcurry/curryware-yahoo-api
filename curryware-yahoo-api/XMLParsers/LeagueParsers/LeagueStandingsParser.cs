using System.Xml;
using System.Xml.Linq;
using curryware_yahoo_api.TeamModels;

namespace curryware_yahoo_api.XMLParsers.LeagueParsers;

public class LeagueStandingsParser
{
    public List<LeagueStandingsTeamModel> GetLeagueStandings(string xmlPayload)
    {
        var xmlDoc = XDocument.Parse(xmlPayload);
        XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
        var standingsNode = xmlDoc.Root?.Descendants(fantasyNameSpace + "standings");
        
        var leagueStandings = new List<LeagueStandingsTeamModel>();

        if (standingsNode != null)
        {
            var allTeams = standingsNode.Descendants(fantasyNameSpace + "team");
            foreach (var currentTeam in allTeams)
            {
                var leagueStandingsTeamModel = new LeagueStandingsTeamModel();
                foreach (var currentElement in currentTeam.Elements())
                {
                    if (currentElement.Name.LocalName == "name")
                        leagueStandingsTeamModel.TeamName = currentElement.Value;
                    
                    if (currentElement.Name.LocalName == "team_key")
                        leagueStandingsTeamModel.TeamKey = currentElement.Value;
                    
                    if (currentElement.Name.LocalName == "team_logos")
                    {
                        var logoSizeElement = currentElement.Descendants(fantasyNameSpace + "url");
                        leagueStandingsTeamModel.TeamLogo = logoSizeElement.FirstOrDefault()?.Value;
                    }
                    
                    if (currentElement.Name.LocalName == "managers")
                    {
                        var managerNameElement = currentElement.Descendants(fantasyNameSpace + "nickname");
                        leagueStandingsTeamModel.ManagerNickName = managerNameElement.FirstOrDefault()?.Value;
                        var managerImageElement = currentElement.Descendants(fantasyNameSpace + "image_url");
                        leagueStandingsTeamModel.ManagerImageUrl = managerImageElement.FirstOrDefault()?.Value;
                        var managerFeloScoreElement = currentElement.Descendants(fantasyNameSpace + "felo_score");
                        var feloValue = managerFeloScoreElement.FirstOrDefault()?.Value;
                        if (feloValue != null)
                            leagueStandingsTeamModel.ManagerFeloScore = Int32.Parse(feloValue);
                        else
                            leagueStandingsTeamModel.ManagerFeloScore = 0;
                    }

                    if (currentElement.Name.LocalName == "team_points")
                    {
                        var totalPointsElement = currentElement.Descendants(fantasyNameSpace + "total");
                        var totalPointsValue = totalPointsElement.FirstOrDefault()?.Value;
                        if (totalPointsValue != null)
                            leagueStandingsTeamModel.TeamTotalPoints = decimal.Parse(totalPointsValue);
                        else
                            leagueStandingsTeamModel.TeamTotalPoints = 0;
                    }

                    if (currentElement.Name.LocalName == "team_standings")
                    {
                        var winsElement = currentElement.Descendants(fantasyNameSpace + "wins");
                        var totalWinsValue = winsElement.FirstOrDefault()?.Value;
                        if (totalWinsValue != null)
                            leagueStandingsTeamModel.TeamWins = Int32.Parse(totalWinsValue);
                        else
                            leagueStandingsTeamModel.TeamWins = 0;
                        
                        var lossesElement = currentElement.Descendants(fantasyNameSpace + "losses");
                        var totalLossesValue = lossesElement.FirstOrDefault()?.Value;
                        if (totalLossesValue != null)
                            leagueStandingsTeamModel.TeamLosses = Int32.Parse(totalLossesValue);
                        else
                            leagueStandingsTeamModel.TeamLosses = 0;
                        
                        var rankElement = currentElement.Descendants(fantasyNameSpace + "rank");
                        var rankValue = rankElement.FirstOrDefault()?.Value;
                        if (rankValue != null)
                            leagueStandingsTeamModel.TeamRank = Int32.Parse(rankValue);
                        else
                            leagueStandingsTeamModel.TeamRank = 0;
                    }
                }
                leagueStandings.Add(leagueStandingsTeamModel);
            }
        }
        
        return leagueStandings;
    }
}