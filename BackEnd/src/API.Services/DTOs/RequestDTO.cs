using API.DataAccess.Models;

namespace API.Services.DTOs
{
    public class RequestDTO
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
    }
}
