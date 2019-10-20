
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MRKTPL.Common;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MRKTPL.Controllers
{
    [Route("api/[controller]")]
    public class UploadImageController : Controller
    {
        private readonly IHostingEnvironment environment;
        public UploadImageController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> Get()
        {
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Conflict
            };
            return response;

        }

        [AllowAnonymous]
        public async Task<HttpResponseMessage> UploadImage(HttpContext context)
        {

            ImageManager imgMng = new ImageManager();
            string ext = System.IO.Path.GetExtension(context.Request.Form.Files[0].FileName);
            string fileName = Path.GetFileName(context.Request.Form.Files[0].FileName);
            if (context.Request.Form.Files[0].FileName.LastIndexOf("\\") != -1)
            {
                fileName = context.Request.Form.Files[0].FileName.
                    Remove(0, context.Request.Form.Files[0].FileName.LastIndexOf("\\")).ToLower();
            }
            try
            {
                IFormFile postedFile = context.Request.Form.Files["userfile"];
                fileName = imgMng.CreateImageFileName(postedFile);
                string ImageName = fileName;
                string FolderName = "";
                FolderName = environment.WebRootPath + "~/BannerImagesFolder";

            }
            catch (Exception)
            {

                throw;
            }
            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Conflict
            };
            return response;
        }


        [HttpPost]
        [Route("UploadFile")]
        public IActionResult Post()
        {
            ImageManager imageManager = new ImageManager();
            bool imgresult = false;
            string status = "";

            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = environment.WebRootPath + "~/BannerImagesFolder";
                string webRootPath = environment.WebRootPath;
                //System.Drawing.Image imageToTransfer = Image.FromFile(file.ToString());
                // System.Drawing.Image imageToTransfer = System.Drawing.Image.FromStream(img);
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);

                    //status = imageManager.CreateBannerImageThumbnail(imageToTransfer, fileName, "banners");

                    imgresult = imageManager.SavePicture(Convert.ToString(Request.Form.Files[0]), fileName, fullPath);

                   

                    //using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    //{
                    //    file.CopyTo(stream);
                    //}
                }
                return Json("Upload Successful.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost, DisableRequestSizeLimit]
        //[Route("UploadFile")]
        public ActionResult UploadFile()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                string folderName = "BannerImagesFolder";
                string webRootPath = environment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }
    }
}
