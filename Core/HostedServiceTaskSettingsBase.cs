namespace Core.Settings;
public abstract class HostedServiceTaskSettingsBase
{
    public bool Active { get; set; }
    public string CronExpressionTimer { get; set; }
}