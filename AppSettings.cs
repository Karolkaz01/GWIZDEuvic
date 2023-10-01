namespace Gwizd;

public class AppSettings
{
    public AwsSettings AwsSettings { get; set; } = null!;
}

public class AwsSettings
{
    public string Key { get; set; } = null!;
    public string Secret { get; set; } = null!;
}
