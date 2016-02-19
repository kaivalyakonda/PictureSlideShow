using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PictureSlide.Models;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace PictureSlide.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(UploadImageModel uploadImageModel)
        {
            if (ModelState.IsValid)
            {
                Bitmap original = null;
                var name = "newimagefile";
                var errorField = string.Empty;

                if (uploadImageModel.IsFile)                
                {
                    errorField = "File";
                    name = Path.GetFileNameWithoutExtension(uploadImageModel.File.FileName);
                    original = Bitmap.FromStream(uploadImageModel.File.InputStream) as Bitmap;
                }

                if (original != null)
                {
                    var fn = Server.MapPath("~/Content/img/" + name + ".png");
                    var img = CreateImage(original, uploadImageModel.X, uploadImageModel.Y, uploadImageModel.Width, uploadImageModel.Height);
                    img.Save(fn, System.Drawing.Imaging.ImageFormat.Png);
                    return RedirectToAction("Index");
                }
                else
                    ModelState.AddModelError(errorField, "Your upload did not seem valid. Please try again using only correct images!");
            }

            return View(uploadImageModel);
        }

        Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return img;
        }
    }
}
