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
        private static IEnumerable<ImageViewModel> _images;

        public ActionResult Index()
        {
            //--- Obtem da Tabela ---
            TableService tableService = new TableService();
            _images = tableService.GetAllImages().Select(url => new ImageViewModel() { Url = url });
            //-----------------------
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
                ImageViewModel image = new ImageViewModel();
                image.Url = imageUrl;

                //--- Add to StorageTable ---
                TableService tableService = new TableService();
                tableService.AddImage(image.Url);
                _images = tableService.GetAllImages().Select(url => new ImageViewModel() { Url = url });
                //---------------------------

            }
            return View("Index", _images);
        }
    }
}