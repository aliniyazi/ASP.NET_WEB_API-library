using API.Services.Requests;
using API.Services.Responses;
using System.Threading.Tasks;

namespace API.Services.ServiceContracts
{
    public interface IUserService
    {
        Task<AuthenticateResponse> LoginAsync(LoginUserRequest userInput);
        Task<AuthenticateResponse> RegisterAsync(RegisterUserRequest request);
        Task<int> GetNumOfUsersAsync();
        Task<Response<string>> RegisterLibrarianAsync(RegisterUserRequest request);
        Task LogoutAsync();
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ChangePasswordAsync(string email, string password);
    }
}
