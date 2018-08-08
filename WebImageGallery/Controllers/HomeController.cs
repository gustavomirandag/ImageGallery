using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebImageGallery.Models;
using StorageService;
using System.Threading.Tasks;

namespace WebImageGallery.Controllers
{
    public class HomeController : Controller
    {
        private static List<Image> _images;

        public HomeController()
        {
            if (_images == null)
                _images = new List<Image>();
        }

        public ActionResult Index()
        {
            return View(_images);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadImages()
        {
            var files = Request.Files;
            var blobService = new BlobService();
            for (int i=0; i<files.Count; i++)
            {
                var imageFile = files[i];
                string imageUrl = await blobService.UploadImage("gallerycontainer",
                    imageFile.FileName,
                    imageFile.InputStream,
                    imageFile.ContentType);
                Image image = new Image();
                image.Url = imageUrl;
                _images.Add(image);
            }
            return View("Index", _images);
        }
    }
}