using Microsoft.AspNetCore.Http;

namespace API.Services.Requests
{
    public class CreateBookRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Genre { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
    }
}
