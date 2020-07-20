using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        /// <summary>
        /// Обработчик файла
        /// </summary>
        private readonly IFileHandlerService _fileHandler;

        public UploadController(IFileHandlerService fileHandler)
        {
            _fileHandler = fileHandler;
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(UploadFile))]
        public async Task<FileUploadResult> UploadFile(IFormFile file)
        {
            return await _fileHandler.SaveFile(file);
        }
    }
}
