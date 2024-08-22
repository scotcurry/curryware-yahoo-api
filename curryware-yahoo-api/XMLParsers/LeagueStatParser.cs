using System.Xml.Linq;
using curryware_yahoo_api.LeagueSettingsModel;

namespace curryware_yahoo_api.XMLParsers;

public class LeagueStatParser
{
    public List<LeagueStatsModel> GetLeagueStatNamesAndValues(string xmlPayload)
    {
        var xmlDoc = XDocument.Parse(xmlPayload);
        XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";
        var statsNode = xmlDoc.Descendants(fantasyNameSpace + "stat_categories");
        var statCategoriesNode = statsNode.Descendants(fantasyNameSpace + "stats");
        var statCategories = statCategoriesNode.Descendants(fantasyNameSpace + "stat");

        var leagueStatWithoutValue = ParseLeagueStatModel(statCategories, fantasyNameSpace);

        var leagueKey = string.Empty;
        var leagueNode = xmlDoc.Root?.Element(fantasyNameSpace + "league");
        if (leagueNode != null)
        {
            var leagueKeyNode = leagueNode.Element( fantasyNameSpace + "league_key");
            if (leagueKeyNode != null)
            {
                leagueKey = leagueKeyNode.Value;
            }
        }

        var statsModifierNode = xmlDoc.Descendants(fantasyNameSpace + "stat_modifiers");
        var statModifierStatsNode = statsModifierNode.Descendants(fantasyNameSpace + "stat");
        var leagueWithValues = ParseStatValueNode(leagueStatWithoutValue, statModifierStatsNode,
            fantasyNameSpace, leagueKey);
        
        return leagueWithValues;
    }

    private List<LeagueStatsModel> ParseLeagueStatModel(IEnumerable<XElement> statCategories, XNamespace fantasyNameSpace)
    {
        var statCategoryList = new List<LeagueStatsModel>();
        // var statModifierNode = xmlDoc.Descendants(fantasyNameSpace + "stat_modifiers");
        
        foreach (var currentCategory in statCategories)
        {
            int statId = 0;
            int statEnabled = 0;
            string statName = String.Empty;
            string statGroup = String.Empty;
            
            var statIdElement = currentCategory.Element(fantasyNameSpace + "stat_id");
            if (statIdElement != null)
            {
                statId = int.Parse(statIdElement.Value);
            }

            var statEnabledElement = currentCategory.Element(fantasyNameSpace + "enabled");
            if (statEnabledElement != null)
            {
                statEnabled = int.Parse(statEnabledElement.Value);
            }

            var statNameElement = currentCategory.Element(fantasyNameSpace + "name");
            if (statNameElement != null)
            {
                statName = statNameElement.Value;
            }

            var statGroupElement = currentCategory.Element(fantasyNameSpace + "group");
            if (statGroupElement != null)
            {
                statGroup = statGroupElement.Value;
            }

            var positionType = GetStatPositionType(currentCategory, fantasyNameSpace);

            var leagueStatModel = new LeagueStatsModel()
            {
                StatId = statId,
                StatEnabled = statEnabled,
                StatName = statName,
                StatGroup = statGroup,
                PositionType = positionType
            };
            statCategoryList.Add(leagueStatModel);
        }

        return statCategoryList;
    }

    private List<LeagueStatsModel> ParseStatValueNode(List<LeagueStatsModel> leagueStatsModel, IEnumerable<XElement> 
        statModifierStatsNode, XNamespace fantasyNameSpace, string leagueKey)
    {
        foreach (var statElement in statModifierStatsNode)
        {
            var statIdElement = statElement.Element(fantasyNameSpace + "stat_id");
            var valueElement = statElement.Element(fantasyNameSpace + "value");

            var statId = 0;
            decimal statValue = 0;
            
            if (statIdElement != null)
            {
                statId = int.Parse(statIdElement.Value);
            }

            if (valueElement != null)
            {
                statValue = decimal.Parse(valueElement.Value);
            }

            LeagueStatsModel? statToUpdate = leagueStatsModel.Find(p => p.StatId == statId);
            if (statToUpdate != null)
            {
                statToUpdate.StatValue = statValue;
                statToUpdate.StatLeagueKey = leagueKey;
            }
        }

        return leagueStatsModel;
    }

    // The Stat Position is in a sub-node, so this pull it out.
    private string GetStatPositionType(XElement currentCategory, XNamespace fantasyNameSpace)
    {
        var positionType = String.Empty;
        var positionTypeElement = currentCategory.Element(fantasyNameSpace + "position_type");
        if (positionTypeElement != null)
        {
            positionType = positionTypeElement.Value;
        }

        return positionType;
    }
}