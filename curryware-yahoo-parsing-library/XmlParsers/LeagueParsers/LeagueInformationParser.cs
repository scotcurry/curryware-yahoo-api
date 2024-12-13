using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

using curryware_yahoo_parsing_library.LeagueModels;
using curryware_yahoo_parsing_library.LogHandler;

namespace curryware_yahoo_parsing_library.XmlParsers.LeagueParsers;

abstract class LeagueInformationParser
{
    internal static string GetLeagueInformationFromXml(string xmlPayload)
    {
        var leagueKey = "Unknown";
        var leagueId = 0;
        var leagueName = "Unknown";
        var leagueUrl = "Unknown";
        var leagueDraftStatus = "Unknown";
        var leagueNumTeams = 0;
        Int64 leagueUpdateTimeStamp = 0;
        var leagueScoringType = "Unknown";
        var leagueWeek = 0;
        var leagueStartWeek = 0;
        var leagueEndWeek = 0;
        var leagueIsFinished = false;
        var leagueSeason = 0;
        var leagueRenewCode = "Unknown";
        var allowDisableList = false;

        try
        {
            var xmlDoc = XDocument.Parse(xmlPayload);
            XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";

            var leagueSettings = xmlDoc.Descendants(fantasyNameSpace + "league");
            var leagueNode = leagueSettings.First();

            var leagueKeyElement = leagueNode.Element(fantasyNameSpace + "league_key");
            if (leagueKeyElement != null)
            {
                leagueKey = leagueKeyElement.Value;
            }

            var leagueIdElement = leagueNode.Element(fantasyNameSpace + "league_id");
            if (leagueIdElement != null)
            {
                leagueId = Convert.ToInt32(leagueIdElement.Value);
            }

            var leagueNameElement = leagueNode.Element(fantasyNameSpace + "name");
            if (leagueNameElement != null)
            {
                leagueName = leagueNameElement.Value;
            }

            var leagueUrlElement = leagueNode.Element(fantasyNameSpace + "url");
            if (leagueUrlElement != null)
            {
                leagueUrl = leagueUrlElement.Value;
            }

            var leagueDraftStatusElement = leagueNode.Element(fantasyNameSpace + "draft_status");
            if (leagueDraftStatusElement != null)
            {
                leagueDraftStatus = leagueDraftStatusElement.Value;
            }

            var leagueNumTeamsElement = leagueNode.Element(fantasyNameSpace + "num_teams");
            if (leagueNumTeamsElement != null)
            {
                leagueNumTeams = Convert.ToInt32(leagueNumTeamsElement.Value);
            }

            var leagueUpdateTimeStampElement = leagueNode.Element(fantasyNameSpace + "league_update_timestamp");
            if (leagueUpdateTimeStampElement != null)
            {
                leagueUpdateTimeStamp = Convert.ToInt64(leagueUpdateTimeStampElement.Value);
            }

            var leagueScoringTypeElement = leagueNode.Element(fantasyNameSpace + "scoring_type");
            if (leagueScoringTypeElement != null)
            {
                leagueScoringType = leagueScoringTypeElement.Value;
            }

            var leagueWeekElement = leagueNode.Element(fantasyNameSpace + "week");
            if (leagueWeekElement != null)
            {
                leagueWeek = Convert.ToInt32(leagueWeekElement.Value);
            }

            var leagueStartWeekElement = leagueNode.Element(fantasyNameSpace + "start_week");
            if (leagueStartWeekElement != null)
            {
                leagueStartWeek = Convert.ToInt32(leagueStartWeekElement.Value);
            }

            var leagueEndWeekElement = leagueNode.Element(fantasyNameSpace + "end_week");
            if (leagueEndWeekElement != null)
            {
                leagueEndWeek = Convert.ToInt32(leagueEndWeekElement.Value);
            }

            var leagueIsFinishedElement = leagueNode.Element(fantasyNameSpace + "is_final_standings");
            if (leagueIsFinishedElement != null)
            {
                leagueIsFinished = Convert.ToBoolean(leagueIsFinishedElement.Value);
            }

            var leagueSeasonElement = leagueNode.Element(fantasyNameSpace + "season");
            if (leagueSeasonElement != null)
            {
                leagueSeason = Convert.ToInt32(leagueSeasonElement.Value);
            }

            var leagueRenewCodeElement = leagueNode.Element(fantasyNameSpace + "renew");
            if (leagueRenewCodeElement != null)
            {
                leagueRenewCode = leagueRenewCodeElement.Value;
            }

            var allowDisableListElement = leagueNode.Element(fantasyNameSpace + "allow_add_to_dl_extra_pos");
            if (allowDisableListElement != null)
            {
                var allowDisableListValue = Convert.ToInt32(allowDisableListElement.Value);
                allowDisableList = Convert.ToBoolean(allowDisableListValue);
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

        var leagueInformation = new LeagueInformationModel
        {
            LeagueKey = leagueKey,
            LeagueId = leagueId,
            LeagueName = leagueName,
            LeagueUrl = leagueUrl,
            DraftStatus = leagueDraftStatus,
            NumTeams = leagueNumTeams,
            LeagueUpdateTimeStamp = leagueUpdateTimeStamp,
            ScoringType = leagueScoringType,
            CurrentWeek = leagueWeek,
            StartWeek = leagueStartWeek,
            EndWeek = leagueEndWeek,
            IsFinished = leagueIsFinished,
            Season = leagueSeason,
            RenewCode = leagueRenewCode,
            AllowDisableList = allowDisableList
        };

        try
        {
            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = false,
            };

            var json = JsonSerializer.Serialize(leagueInformation, serializerOptions);
            return json;
        }
        catch (ArgumentNullException argumentNullException)
        {
            CurrywareLogHandler.AddLog(argumentNullException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to serialize league information", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
        catch (InvalidCastException invalidCastException)
        {
            CurrywareLogHandler.AddLog(invalidCastException.Message, LogLevel.Error);
            CurrywareLogHandler.AddLog("Error: Failed to serialize league information", LogLevel.Error);
            return "Error: Failed to serialize league information";
        }
    }
}