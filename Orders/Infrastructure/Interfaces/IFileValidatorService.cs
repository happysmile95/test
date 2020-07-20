using Microsoft.AspNetCore.Http;

namespace Infrastructure
{
    /// <summary>
    /// Сервис для валидация файла
    /// </summary>
    public interface IFileValidatorService
    {
        /// <summary>
        /// Проверить загруженный файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        string ValidateUploadedFile(IFormFile file);

        /// <summary>
        /// Проверить консистентность данных
        /// </summary>
        void CheckConsistencyData();
    }
}
