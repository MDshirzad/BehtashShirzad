using BehtashShirzad.Models.Base;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BehtashShirzad.Model.ApiModels
{
    public class UserRegistrationDto : IBaseDto
    {
        //public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
     
        public string PhoneNumber { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
