using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Infrastructure;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IFileHandler _fileHandler;

        public UploadController(IFileHandler fileHandler)
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
            return await _fileHandler.WriteFile(file);
        }
    }
}
