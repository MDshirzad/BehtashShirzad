using BehtashShirzad.Models.Base;
using BehtashShirzad.Tools;
using Newtonsoft.Json;

namespace BehtashShirzad.Models.ApiModels
{
    public class UserLoginDto : IBaseDto
    {
        public string? Credential { get; set; }
       
        public   string Password { get; set; }

        public override string ToString()
        {
            this.Password = Infrastructure.EncodeForSafety(Infrastructure.CreatePassHash( this.Password));
            this.Credential = Infrastructure.EncodeForSafety(this.Credential);
            return JsonConvert.SerializeObject(this);
        }

    }
}
