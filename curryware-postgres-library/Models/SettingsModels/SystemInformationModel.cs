namespace curryware_postgres_library.Models.SettingsModels;

public class SystemInformationModel
{
    public int SystemId { get; set; }
    public string? SettingName { get; set; }
    public string? SettingValue { get; set; }
}