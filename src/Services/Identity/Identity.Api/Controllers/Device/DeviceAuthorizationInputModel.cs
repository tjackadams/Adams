using Adams.Services.Identity.Api.Controllers.Consent;

namespace Adams.Services.Identity.Api.Controllers.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}