using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        BlobStorageService _blobStorageService = new BlobStorageService();

        public ActionResult Upload()
        {
            CloudBlobContainer blobContainer = _blobStorageService.GetCloudBlobContainer();
            List<string> blobs = new List<string>();
            foreach (var blobItem in blobContainer.ListBlobs())
            {
                blobs.Add(blobItem.Uri.ToString());
            }
            return View(blobs);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase image)
        {
            if (image.ContentLength > 0)
            {
                CloudBlobContainer blobContainer = _blobStorageService.GetCloudBlobContainer();
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(image.FileName);
                blob.UploadFromStream(image.InputStream);
            }
            return RedirectToAction("Upload");
        }
        [HttpPost]
        public string DeleteImage(string Name)
        {
            Uri uri = new Uri(Name);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);

            CloudBlobContainer blobContainer = _blobStorageService.GetCloudBlobContainer();
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);

            blob.Delete();
            return "File Delete";
        }
    }
}