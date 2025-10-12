using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Utility.Services.Filters;

namespace Utility.Controllers
{
    [Produces("application/json")]
    [Route("api/Uploader/[action]")]
    public class UploaderController : Controller {
        private IWebHostEnvironment _environment;

        public UploaderController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("/api/Uploader/Upload")]
        [Produces("application/json")]
        [TypeFilter(typeof(AllowedExtensionsFilterAttribute), Arguments = new object[] { new[] { ".jpg", ".png", ".jpeg", ".png" } })]
        public async Task<JsonResult> Upload(IFormFile file)
        {
            string imagePath = "uploads/images";

            string originalFileName = Path.GetFileName(file.FileName);
            FileInfo fi = new FileInfo(file.FileName);
            var date = DateTime.Now.ToString("hhmmssffffff");
            var uploads = Path.Combine(_environment.WebRootPath, imagePath);
            var setFilename = "Image" + date + originalFileName.ToSlug() + fi.Extension;
            var filePath = Path.Combine(uploads, setFilename);
            FileStream fileStream = new FileStream(filePath,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.None);
            await file.CopyToAsync(fileStream);
            try
            {
                await fileStream.DisposeAsync();
                fileStream.Close();
                Uploader upload = new Uploader();

                upload.link = "/" + imagePath + "/" + setFilename;
                return Json(upload);
            }
            catch (Exception e)
            {
                return Json(e);
            }

        }

        //[HttpPost]
        //[TypeFilter(typeof(AllowedExtensionsFilterAttribute), Arguments = new object[] { new string[] { ".mp4" ,".webm",".ogg", ".mov"} })]
        //[RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        //public async Task<IActionResult> UploadVideoFile(IFormFile file)
        //{
        //    try
        //    {
        //        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
        //        var result = await Config.PutObject(file, Config.AWSFilePath + fileName, Config.AWSBucketAssets);
        //        if (result)
        //        {
        //            var _link = Config.AWSFilePath + fileName;
        //            Uploader upload = new Uploader();
        //            upload.ex = Path.GetExtension(_link);
        //            upload.link = _link;
        //            return Json(upload);
        //        }
        //        else
        //        {
        //            return Json(new { error = "error" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ex });
        //    }

        //}
        
        //[HttpPost]
        //[TypeFilter(typeof(AllowedExtensionsFilterAttribute), Arguments = new object[] { new string[] { ".png",".jpg",".jpeg",".webp" ,".pdf",".doc", ".docx"} })]
        //[RequestSizeLimit(2 * 1024 * 1024)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 2 * 1024 * 1024)]// 2MB
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    try
        //    {
        //        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
        //        var result = await Config.PutObject(file, Config.AWSFilePath + fileName, Config.AWSBucketAssets);
        //        if (result)
        //        {
        //            var _link = Config.AWSFilePath + fileName;
        //            Uploader upload = new Uploader();
        //            upload.ex = Path.GetExtension(_link);
        //            upload.link = _link;
        //            return Json(upload);
        //        }
        //        else
        //        {
        //            return Json(new { error = "error" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ex });
        //    }

        //}




        [HttpPost]
        [TypeFilter(typeof(AllowedExtensionsFilterAttribute), Arguments = new object[] { new string[] { ".png", ".pdf", ".bmp", ".svg", ".gif" } })]
        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        public async Task<IActionResult> UploadPDFFile(IFormFile file)
        {
            string filePath = "uploads/files";
            FileInfo fi = new FileInfo(file.FileName);

            string originalFileName = Path.GetFileName(file.FileName);
            var date = DateTime.Now.ToString("hhmmssffffff");
            var uploads = Path.Combine(_environment.WebRootPath, filePath);
            var setFilename = "File" + date + originalFileName.ToSlug() + fi.Extension;
            var setFilePath = Path.Combine(uploads, setFilename);

            if (file.Length > 0)
            {

                using (var stream = new FileStream(setFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    await file.CopyToAsync(stream);
                    await stream.DisposeAsync();
                    stream.Close();
                }
            }

            try
            {
                Uploader upload = new Uploader();
                upload.ex = Path.GetExtension(originalFileName);
                upload.link = "/" + filePath + "/" + setFilename;
                return Json(upload);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }


        //[HttpPost]
        //[TypeFilter(typeof(AllowedExtensionsFilterAttribute), Arguments = new object[] { new string[] { ".png"} })]
        //[RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        //[RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
        //public async Task<IActionResult> UploadPNGFile(IFormFile file)
        //{
        //    try
        //    {
        //        var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
        //        var result = await Config.PutObject(file, Config.AWSFilePath + fileName, Config.AWSBucketAssets);
        //        if (result)
        //        {
        //            var _link = Config.AWSFilePath + fileName;
        //            Uploader upload = new Uploader();
        //            upload.ex = Path.GetExtension(_link);
        //            upload.link = _link;
        //            return Json(upload);
        //        }
        //        else
        //        {
        //            return Json(new { error = "error" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { ex });
        //    }

        //}

    }

    public class Uploader
    {
        public string? link { get; set; }
        public string? ex { get; set; }
    }
}
