using curryware_postgres_library.SettingsQueries;

namespace curryware_postgres_library_tests;

public class SystemInformationTest
{
    [Fact]
    public async Task GetSystemInformationTest()
    {
        Environment.SetEnvironmentVariable("POSTGRES_USERNAME", "scot");
        Environment.SetEnvironmentVariable("POSTGRES_PASSWORD", "AirWatch1");
        Environment.SetEnvironmentVariable("POSTGRES_PORT", "5432");
        Environment.SetEnvironmentVariable("POSTGRES_DATABASE", "currywarefantasy");
        Environment.SetEnvironmentVariable("POSTGRES_SERVER", "localhost");
        var systemInformation= await PostgresSettingQueries.GetSystemInformation();
        Assert.Contains("41762", systemInformation);
    }
}