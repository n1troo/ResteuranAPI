using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ResteuranAPI.Controllers
{
    [Route("/api/file")]
    [ApiController]
    public class FileController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();

            var filePath = Path.Combine(rootPath, "PrivateFiles", fileName);

            var fileExist = System.IO.File.Exists(filePath);

            if(!fileExist)
            {
                NotFound();
            }

            var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            fileExtensionContentTypeProvider.TryGetContentType(filePath, out string contentType);


            var fileContent = System.IO.File.ReadAllBytes(filePath);

            return File(fileContent, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file )
        {
            if(file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
               
                var fullPath = Path.Combine(rootPath, "PrivateFiles", file.FileName);
                using(var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    file.CopyTo(stream);
                }

                return Ok();

            }

            return BadRequest();
        }
    }
}
