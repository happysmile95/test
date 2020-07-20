using Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    /// <summary>
    /// Интерфейс для проверки файла
    /// </summary>
    public class FileValidator : IFileValidator
    {
        public string Validate(IFormFile file)
        {
            if (file == null)
            {
                return "Файл не выбран.";
            }
            if (!file.FileName.Contains("xlsx"))
            {
                return "Некорректный файл. Необходим файл с расширением *.xlxs";
            }
            if (file.Length == 0)
            {
                return "Файл пуст";
            }
            return null; 
        }
    }
}
