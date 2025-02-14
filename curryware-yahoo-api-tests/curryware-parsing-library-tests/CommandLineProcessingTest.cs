using Xunit;
using curryware_fantasy_command_line_tool.CommandLineHandlers;
using curryware_fantasy_command_line_tool.CommandLineModels;

namespace curryware_yahoo_api_tests.curryware_parsing_library_tests;

public class CommandLineProcessingTest
{
    [Fact]
    public void InvalidTopLevelParameterTest()
    {
        var commandLineArgs = new[] { "stat", "player" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("There is a valid command line argument, test should have failed.");
        }
        catch (InvalidParameterException exception)
        {
            Assert.Equal("No Top Level Command Provided.", exception.Message);
        }
    }
    
    [Fact]
    public void InvalidGameIdNoIdTest()
    {
        var commandLineArgs = new[] { "stats", "-g" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, no game id provided.");
        }
        catch (InvalidParameterException exception)
        {
            Assert.Equal("A gameID must be provided.", exception.Message);
        }
    }

    [Fact]
    public void InvalidGameIdWrongFormatTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "1234567890" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, game id is not a valid format.");
        }
        catch (InvalidParameterException exception)
        {
            Assert.Equal("Game ID must be a 3 digits.", exception.Message);
        }
    }

    [Fact]
    public void InvalidLeagueIdNoIdTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "423", "-l" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, no league id provided.");
        }
        catch (InvalidParameterException exception)
        {
            Assert.Equal("A leagueId must be provided.", exception.Message);
        }
    }

    [Fact]
    public void InvalidLeagueFormatTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "423", "-l", "scot" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, invalid league format.");
        }
        catch (InvalidOptionException exception)
        {
            Assert.Equal("League ID must be between 2 and 6 digits.", exception.Message);
        }
    }

    [Fact]
    public void ValidGameAndLeagueTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "449", "-l", "483521" };
        var returnObject = CommandLineParser.ParseCommandLine(commandLineArgs);
        var returnType = returnObject.GetType();
        if (returnType != typeof(GameStatsCommandLineParameters))
            Assert.Fail("Didn't get a valid return type.");
        else
            Assert.True(true);
    }

    [Fact]
    public void InvalidWeekTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "449", "-l", "483521", "-w", "scot" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
        }
        catch (InvalidOptionException invalidOptionException)
        {
            Assert.Equal("Week must be between 1 and 17.", invalidOptionException.Message);
        }
    }

    [Fact]
    public void ValidWeekTest()
    {
        var commandLineArgs = new[] { "stats", "-g", "449", "-l", "483521", "-w", "10" };
        var returnObject = CommandLineParser.ParseCommandLine(commandLineArgs);
        var returnType = returnObject.GetType();
        if (returnType != typeof(GameStatsCommandLineParameters))
            Assert.Fail("Didn't get a valid return type.");
        else
            Assert.True(true);
    }

    [Fact]
    public void ValidPlayerTest()
    {
        var commandLineArgs = new[] { "players" };
        var returnObject = CommandLineParser.ParseCommandLine(commandLineArgs);
        var returnType = returnObject.GetType();
        if (returnType != typeof(PlayerCommandLineParameters))
            Assert.Fail("Didn't get a valid return type.");
        else
            Assert.True(true);
    }

    [Fact]
    public void InvalidPositionTest()
    {
        var commandLineArgs = new[] { "players", "-P", "scot" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, invalid position.");
        }
        catch (InvalidOptionException invalidOptionException)
        {
            Assert.Equal("Position must be [QB, RB, WR, TE, K, or D]", invalidOptionException.Message);
        }
    }

    [Fact]
    public void InvalidStatusTest()
    {
        var commandLineArgs = new[] { "players", "-P", "qb", "-s", "scot" };
        try
        {
            CommandLineParser.ParseCommandLine(commandLineArgs);
            Assert.Fail("This should have failed, invalid position.");
        }
        catch (InvalidOptionException invalidOptionException)
        {
            Assert.Equal("Status must be [A, FA, W, or T]", invalidOptionException.Message);
        }
    }
}