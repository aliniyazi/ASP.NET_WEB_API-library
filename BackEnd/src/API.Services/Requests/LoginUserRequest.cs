using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Services.Requests
{
    public class LoginUserRequest 
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [JsonIgnore]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
