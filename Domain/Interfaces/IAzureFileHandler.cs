using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces
{
    public interface IAzureFileHandler
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}