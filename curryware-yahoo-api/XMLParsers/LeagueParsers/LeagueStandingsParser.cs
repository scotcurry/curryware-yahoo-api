using System.Xml;
using System.Xml.Linq;
using curryware_yahoo_api.TeamModels;
using Serilog;
using Serilog.Formatting.Json;

namespace curryware_yahoo_api.XMLParsers.LeagueParsers;

public class LeagueStandingsParser
{
    public List<LeagueStandingsTeamModel> GetLeagueStandings(string xmlPayload)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateLogger();
        
        var leagueStandings = new List<LeagueStandingsTeamModel>();
        try
        {
            var xmlDoc = XDocument.Parse(xmlPayload);
            leagueStandings = ParseLeaguesStandings(xmlDoc);
        }
        catch (XmlException xmlException)
        {
            throw new XmlException("Failed to parse league standings", xmlException);
        }
        
        return leagueStandings;
    }

    private List<LeagueStandingsTeamModel> ParseLeaguesStandings(XDocument xmlDoc)
    {
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
                        leagueStandingsTeamModel.ManagerFeloScore = feloValue != null ? Int32.Parse(feloValue) : 0;
                    }

                    if (currentElement.Name.LocalName == "team_points")
                    {
                        var totalPointsElement = currentElement.Descendants(fantasyNameSpace + "total");
                        var totalPointsValue = totalPointsElement.FirstOrDefault()?.Value;
                        leagueStandingsTeamModel.TeamTotalPoints = totalPointsValue != null ? decimal.Parse(totalPointsValue) : 0;
                    }

                    if (currentElement.Name.LocalName == "team_standings")
                    {
                        var winsElement = currentElement.Descendants(fantasyNameSpace + "wins");
                        var totalWinsValue = winsElement.FirstOrDefault()?.Value;
                        leagueStandingsTeamModel.TeamWins = totalWinsValue != null ? Int32.Parse(totalWinsValue) : 0;

                        var lossesElement = currentElement.Descendants(fantasyNameSpace + "losses");
                        var totalLossesValue = lossesElement.FirstOrDefault()?.Value;
                        leagueStandingsTeamModel.TeamLosses = totalLossesValue != null ? Int32.Parse(totalLossesValue) : 0;

                        var rankElement = currentElement.Descendants(fantasyNameSpace + "rank");
                        var rankValue = rankElement.FirstOrDefault()?.Value;
                        leagueStandingsTeamModel.TeamRank = rankValue != null ? Int32.Parse(rankValue) : 0;
                    }
                }

                leagueStandings.Add(leagueStandingsTeamModel);
            }

        }
        return leagueStandings;
    }
}