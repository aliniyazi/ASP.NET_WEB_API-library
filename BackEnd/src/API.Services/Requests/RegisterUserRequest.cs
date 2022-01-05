using System.ComponentModel.DataAnnotations;

namespace API.Services.Requests
{
    public class RegisterUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [StringLength(44, ErrorMessage = "Password must not exceed 44 characters")]
        public string Password { get; set; }
        [StringLength(44, ErrorMessage = "Password must not exceed 44 characters")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }
        [Phone]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number must only contain numbers")]
        public string PhoneNumber { get; set; }
        public AddressRequest Address { get; set; }
    }
}
