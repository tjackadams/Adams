using System.ComponentModel.DataAnnotations;

namespace Nexus.Portal;

public class Settings
{
    public const string Section = nameof(Settings);

    [Required]
    public Uri ApiGatewayUri { get; set; } = null!;
}
