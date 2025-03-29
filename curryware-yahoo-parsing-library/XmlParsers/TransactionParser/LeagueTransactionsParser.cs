using System.Text.Json;
using System.Xml.Linq;
using curryware_yahoo_parsing_library.TransactionModels;
using Serilog;

namespace curryware_yahoo_parsing_library.XmlParsers.TransactionParser;

internal abstract class LeagueTransactionsParser
{
    internal static string GetParseLeagueTransactionsXml(string xmlPayload)
    {
        var transactionList = new List<TransactionModel>();
        var totalTransactions = 0;
        
        var xmlDoc = XDocument.Parse(xmlPayload);
        XNamespace fantasyNameSpace = "http://fantasysports.yahooapis.com/fantasy/v2/base.rng";

        var transactionNodeForCount = xmlDoc.Descendants(fantasyNameSpace + "transactions").FirstOrDefault();
        if (transactionNodeForCount is { HasAttributes: true })
        {
            totalTransactions =int.Parse(transactionNodeForCount.Attribute("count")!.Value);
        }

        var transactionNodes = xmlDoc.Descendants(fantasyNameSpace + "transaction");
        foreach (var currentTransactionNode in transactionNodes)
        {
            var transactionKey = string.Empty;
            var transactionId = 0;
            var transactionType = string.Empty;
            var transactionTime = 0;
            var transactionStatus = string.Empty;
            
            var transactionKeyNode = currentTransactionNode.Descendants(fantasyNameSpace + "transaction_key").FirstOrDefault();
            if (transactionKeyNode != null)
            {
                transactionKey = transactionKeyNode.Value;
            }
            
            var transactionIdNode = currentTransactionNode.Descendants(fantasyNameSpace + "transaction_id").FirstOrDefault();
            if (transactionIdNode != null)
            {
                transactionId = int.Parse(transactionIdNode.Value);
            }
            
            var transactionTypeNode = currentTransactionNode.Descendants(fantasyNameSpace + "type").FirstOrDefault();
            if (transactionTypeNode != null)
            {
                transactionType = transactionTypeNode.Value;
            }
            
            var transactionTimeNode = currentTransactionNode.Descendants(fantasyNameSpace + "timestamp").FirstOrDefault();
            if (transactionTimeNode != null)
            {
                transactionTime = int.Parse(transactionTimeNode.Value);
            }
            
            var transactionStatusNode = currentTransactionNode.Descendants(fantasyNameSpace + "status").FirstOrDefault();
            if (transactionStatusNode != null)
            {
                transactionStatus = transactionStatusNode.Value;
            }

            var playerEffectedNode = currentTransactionNode.Descendants(fantasyNameSpace + "players").FirstOrDefault();
            if (playerEffectedNode == null) continue;
                var playerList = ParsePlayerData(playerEffectedNode, fantasyNameSpace);

            var transactionInfo = new TransactionModel
            {
                TransactionKey = transactionKey,
                TransactionId = transactionId,
                TransactionType = transactionType,
                TransactionStatus = transactionStatus,
                TransactionTimestamp = transactionTime,
                PlayersInvolved = playerList

            };
            
            transactionList.Add(transactionInfo);
        }

        var transactionWithCount = new TransactionListWithCount
        {
            TransactionCount = totalTransactions,
            Transactions = transactionList
        };
        
        var serializerOptions = new JsonSerializerOptions
        {
            WriteIndented = false
        };
        var json = JsonSerializer.Serialize(transactionWithCount, serializerOptions);
        // CurrywareLogHandler.AddLog($"Processed: {totalTransactions} transactions", LogLevel.Debug);
        Log.Debug("\"Processed: {totalTransactions}",  totalTransactions);
        return json;
    }

    private static List<TransactionPlayersInvolved> ParsePlayerData(XElement playerList, XNamespace fantasyNameSpace)
    {
        var playerListInvolved = new List<TransactionPlayersInvolved>();
        
        var playerNodeDescendants = playerList.Descendants(fantasyNameSpace + "player");
        foreach (var currentPlayer in playerNodeDescendants)
        {
            var playerKey = string.Empty;
            var playerId = 0;
            var transactionType = string.Empty;
            var source = string.Empty;
            var destination = string.Empty;
            var destinationTeamKey = string.Empty;
            
            var playerKeyNode = currentPlayer.Descendants(fantasyNameSpace + "player_key").FirstOrDefault();
            if (playerKeyNode != null)
                playerKey = playerKeyNode.Value;
            
            var playerIdNode = currentPlayer.Descendants(fantasyNameSpace + "player_id").FirstOrDefault();
            if (playerIdNode != null)
                playerId = int.Parse(playerIdNode.Value);
            
            var transactionDataNode = currentPlayer.Descendants(fantasyNameSpace + "transaction_data");
            var transactionTypeNode = transactionDataNode.Descendants(fantasyNameSpace + "type").FirstOrDefault();
            if (transactionTypeNode != null)
                transactionType = transactionTypeNode.Value;
            
            var sourceNode = currentPlayer.Descendants(fantasyNameSpace + "source_type").FirstOrDefault();
            if (sourceNode != null)
                source = sourceNode.Value;
            
            var destinationNode = currentPlayer.Descendants(fantasyNameSpace + "destination_type").FirstOrDefault();
            if (destinationNode != null)
                destination = destinationNode.Value;
            
            var destinationTeamKeyNode = currentPlayer.Descendants(fantasyNameSpace + "destination_team_key").FirstOrDefault();
            if (destinationTeamKeyNode != null)
                destinationTeamKey = destinationTeamKeyNode.Value;

            var playerTransactionData = new TransactionPlayersInvolved
            {
                PlayerKey = playerKey,
                PlayerId = playerId,
                TransactionType = transactionType,
                TransactionSource = source,
                DestinationType = destination,
                DestinationTeamId = destinationTeamKey
            };
            playerListInvolved.Add(playerTransactionData);
        }
        
        return playerListInvolved;
    }
}