using Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Интерфейс для валидация файла
    /// </summary>
    public interface IFileValidator
    {
        /// <summary>
        /// Проверить файл
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns></returns>
        string Validate(IFormFile file);
    }
}
