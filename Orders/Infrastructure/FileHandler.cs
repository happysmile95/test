using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class FileHandler : IFileHandler
    {
        private readonly string targetDirectory = "INCOMING";
        private readonly string currentDir = string.Empty;
        private readonly IFileValidator _fileValidator;

        public FileHandler(IFileValidator fileValidator)
        {
            _fileValidator = fileValidator;
            InitializeDirectory();
        }

        private void InitializeDirectory()
        {
            var currentDir = $"{Directory.GetParent(Directory.GetCurrentDirectory())}\\{targetDirectory}";
            if (!Directory.Exists(currentDir))
            {
                Directory.CreateDirectory(currentDir);
            }
        }

        public async Task<FileUploadResult> WriteFile(IFormFile file)
        {
            string error = _fileValidator.Validate(file);
            if(!string.IsNullOrEmpty(error))
            {
                return new FileUploadResult()
                {
                    Error = error,
                    Success = false,
                };
            }
            using (var fileStream = new FileStream($"{currentDir}\\{file.FileName}", FileMode.Create))
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
