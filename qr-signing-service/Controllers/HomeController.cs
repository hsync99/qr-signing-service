using Microsoft.AspNetCore.Mvc;
using qr_signing_service.Models;
using QRCoder;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;

namespace qr_signing_service.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(string QRtext)
        {
            QRCodeGenerator QRGen = new QRCodeGenerator();
            QRCodeData Qrinfo = QRGen.CreateQrCode(QRtext, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCoder = new QRCode(Qrinfo);

            Bitmap QRbitmap = qRCoder.GetGraphic(50);

            // Color 
            //Bitmap QRbitmap = qRCoder.GetGraphic(50, Color.Blue, Color.Gray, true);

            byte[] bitmapArray = bitmaptoArray(QRbitmap);
            var Qrcodeimage = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));
            ViewBag.QRCodeImage = Qrcodeimage;
            return View();
        }

        private static byte[] bitmaptoArray(Bitmap bitmapimage)
        {
            using (MemoryStream mstream = new MemoryStream())
            {

                bitmapimage.Save(mstream, ImageFormat.Png);
                return mstream.ToArray();
            }

        }
    }
}