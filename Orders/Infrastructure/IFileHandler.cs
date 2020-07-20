using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IFileHandler
    {
        Task<FileUploadResult> WriteFile(IFormFile file);
    }
}
