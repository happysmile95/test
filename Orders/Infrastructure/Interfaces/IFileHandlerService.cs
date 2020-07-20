using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для обработки файла
    /// </summary>
    public interface IFileHandlerService
    {
        /// <summary>
        /// Сохранить файл на диск
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        Task<FileUploadResult> SaveFile(IFormFile file);
    }
}
