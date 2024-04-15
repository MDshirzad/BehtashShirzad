
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BehtashShirzad.Models.DbModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }


        [StringLength(250)]
        public string Password { get; set; }
        
        public string Role { get; set; } = "user";
        public bool isVerified { get; set; } = false;
        public bool isAdmin { get; set; } = false;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public List<Invoice> invoices { get; set; }
    }
}
