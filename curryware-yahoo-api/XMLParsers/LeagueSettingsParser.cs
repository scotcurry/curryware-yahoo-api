using System.Xml.Linq;
using curryware_yahoo_api.LeagueSettingsModel;

namespace curryware_yahoo_api.XMLParsers;

public class LeagueSettingsParser
{
    public LeagueNameModel GetLeagueName(string xmlPayload)
    {
        var xmlDoc = XDocument.Parse(xmlPayload);
        XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
        var leagueSettings  = xmlDoc.Descendants(fantasyNameSpace + "league");
        var leagueNode = leagueSettings.First();
        var leagueName = "Unknown";
        var leagueKey = "Unknown";

        var leagueNameElement = leagueNode.Element(fantasyNameSpace + "name");
        if (leagueNameElement != null)
        {
            leagueName = leagueNameElement.Value;
        }

        var leagueKeyElement = leagueNode.Element(fantasyNameSpace + "league_key");
        if (leagueKeyElement != null)
        {
            leagueKey = leagueKeyElement.Value;
        }

        var leagueModel = new LeagueNameModel
        {
            LeagueName = leagueName,
            LeagueKey = leagueKey
        };

        return leagueModel;
    }
}