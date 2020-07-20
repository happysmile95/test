using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для обработки файла
    /// </summary>
    public class FileHandlerService : IFileHandlerService
    {
        /// <summary>
        /// Обработчик файла
        /// </summary>
        private readonly IFileValidatorService _fileValidatorService;

        public FileHandlerService(IFileValidatorService fileValidatorService)
        {
            _fileValidatorService = fileValidatorService;
            FolderTypeHelper.InitializeDirectories($"{Directory.GetParent(Directory.GetCurrentDirectory())}");
        }

        /// <summary>
        /// Сохранить файл на диск
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        public async Task<FileUploadResult> SaveFile(IFormFile file)
        {
            string error = _fileValidatorService.ValidateUploadedFile(file);
            if(!string.IsNullOrEmpty(error))
            {
                return new FileUploadResult()
                {
                    Error = error,
                    Success = false,
                };
            }
            using (var fileStream = new FileStream($"{FolderTypeHelper.GetPath(FolderTypeHelper.InComing)}\\{file.FileName}", FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                return new FileUploadResult()
                {
                    Error = null,
                    Success = true,
                };
            }
        }
    }
}
