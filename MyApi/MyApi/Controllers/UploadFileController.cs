using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadFileController : ControllerBase
    {
        private static string UPLOAD_PATH = "/home/store-data/upload/";

        [HttpPost]
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            string filePath = "null";

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    try
                    {
                        //var filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        //filePath += "/X/" + formFile.FileName;

                        string extension = System.IO.Path.GetExtension(formFile.FileName);
                        string fileName = System.IO.Path.GetFileNameWithoutExtension(formFile.FileName);
                        string time = ((int)DateTime.Now.TimeOfDay.TotalSeconds).ToString() + '-' + DateTime.Now.Day.ToString() + '-' + DateTime.Now.Month.ToString() + '-' + DateTime.Now.Year.ToString();
                        filePath = Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + '/' + fileName + '-' + time + extension;

                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }catch (Exception e)
                    {
                        return Ok(new { filePath, count = files.Count, size, error = e.Message});
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new {  filePath ,count = files.Count, size });
        }
    }
}
