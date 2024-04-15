using BehtashShirzad.Models.Base;
using Newtonsoft.Json;

namespace BehtashShirzad.Models.ApiModels
{
    public class VerifyByOtpDto : IBaseDto
    {

        public string phoneNumber { get; set; }
        public string otp { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
