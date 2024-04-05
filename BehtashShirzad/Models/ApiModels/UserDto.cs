using System.ComponentModel.DataAnnotations;

namespace ElliotStore.Model.ApiModels
{
    public class UserDto
    {
        public string Username { get; set; }

        [MinLength(6), MaxLength(12)]
        public string Password { get; set; }
    }
}
