using System.ComponentModel.DataAnnotations;

namespace Nexus.Portal;

public class Settings
{
    [Required]
    public Uri ApiGatewayUrl { get; set; }
}
