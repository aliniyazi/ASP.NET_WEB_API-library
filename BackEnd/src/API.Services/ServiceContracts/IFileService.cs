using API.Services.DTOs;
using API.Services.Requests;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Services.ServiceContracts
{
    public interface IFileService
    {
        Task<string> UploadImageToAzureStorage(IFormFile image);

        Task DeleteImageFromAzure(string ImageDb);

        Task<string> GetImageFromAzureStorage(string imageName);
    }
}
