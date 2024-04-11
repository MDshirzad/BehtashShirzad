using System.ComponentModel.DataAnnotations;

namespace ElliotStore.Model.ApiModels
{
    public class UserRegistrationDto
    {
        //public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Email { get; set; }
     
        public string PhoneNumber { get; set; }
    }
}
