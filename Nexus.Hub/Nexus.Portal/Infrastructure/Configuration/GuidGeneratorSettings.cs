namespace Nexus.Portal.Infrastructure.Configuration;

public class GuidGeneratorSettings
{
    public const string Key = nameof(GuidGeneratorSettings);
    public bool Uppercase { get; set; } = true;
    public bool Brackets { get; set; } = false;
    public bool Hyphens { get; set; } = true;
}
