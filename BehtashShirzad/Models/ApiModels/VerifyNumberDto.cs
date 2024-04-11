using BehtashShirzad.Models.ApiModels.Base;

namespace BehtashShirzad.Models.ApiModels
{
    public class VerifyNumberDto: BaseUserLogin
    {
        public string? PhoneNumber { get; set; }
    }
}
