using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace qr_signing_service.Controllers
{
    public class QrCodeController : Controller
    {
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
