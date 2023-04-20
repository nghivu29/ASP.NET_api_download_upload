using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadFileController : ControllerBase
    {
        [HttpGet("{fileUrl}")]
        public async Task<IActionResult> OnPostUploadAsync(string fileUrl)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileUrl);
            if (!System.IO.File.Exists(filePath))
                return Ok(new { error = "file not found", filePath });

            var memory = new MemoryStream();

            try
            {
                await using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
            }
            catch (Exception e)
            {
                return Ok(new { error = e.Message, memoryLength = memory.Length,filePath });
            }

            return File(memory, GetContentType(filePath), "this-is-not-virrut" + Path.GetExtension(filePath));
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
