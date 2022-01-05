namespace API.Services.Requests
{
    public class UpdateRequest
    {
        public string UserId { get; set; }
        public int BookId { get; set; }
        public bool RequestApproved { get; set; }
    }
}
