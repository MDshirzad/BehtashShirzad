namespace BehtashShirzad.Models.ApiModels
{
    public class UserLoginDto
    {
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}
