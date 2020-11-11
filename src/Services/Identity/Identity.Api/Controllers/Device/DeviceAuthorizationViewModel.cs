using Adams.Services.Identity.Api.Controllers.Consent;

namespace Adams.Services.Identity.Api.Controllers.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}