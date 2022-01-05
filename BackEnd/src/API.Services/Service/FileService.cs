using API.Services.ServiceContracts;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Services.Service
{
    public class FileService : IFileService
    {
        private readonly BlobServiceClient blobServiceClient;
        public FileService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadImageToAzureStorage(IFormFile image)
        {
            try
            {
                var blobContainer = blobServiceClient.GetBlobContainerClient("bookstorage");
                var guid = Guid.NewGuid();
                var fileName = guid + "/" + image.FileName;
                var blobClient = blobContainer.GetBlobClient(fileName);
                await blobClient.UploadAsync(image.OpenReadStream());
                var imageName = blobClient.Name.ToString();

                return imageName;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> GetImageFromAzureStorage(string imageName)
        {
            try
            {
                var blobContainer = blobServiceClient.GetBlobContainerClient("bookstorage");
                var blobClient = blobContainer.GetBlobClient(imageName);
                var downloadContent = await blobClient.DownloadAsync();

                string text = "";
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await downloadContent.Value.Content.CopyToAsync(memoryStream);
                    text = Convert.ToBase64String(memoryStream.ToArray());
                }

                return string.Format("data:image/jpeg;base64,{0}", text);
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public async Task DeleteImageFromAzure(string imageDB)
        {
            var blobContainer = blobServiceClient.GetBlobContainerClient("bookstorage");
            var blobClient = blobContainer.GetBlobClient(imageDB);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
