namespace API.Services.Requests
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    }
}
