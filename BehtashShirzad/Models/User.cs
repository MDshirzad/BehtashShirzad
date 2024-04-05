using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ElliotStore.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [MinLength(6),MaxLength(12)]
        [StringLength(250)]
        public string Password { get; set; }

    }
}
