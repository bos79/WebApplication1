using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
//using ZXing;
//using ZXing.Common;
//using ZXing.QrCode;
//using ZXing.Client;
//using ZXing.CoreCompat.System.Drawing;




namespace WebApplication1
{
    public class scanBarcode
    {


        public static async void  Main(string[] args)
        {

            ////Bitmap image;
            //// create a barcode reader instance
            //scanBarcode reader = new scanBarcode();
            //// create a barcode reader instance
            //var barcodeReader = new QRCodeReader();
            //BarcodeReader reader2 = new BarcodeReader();

            //var coreCompatReader = new ZXing.CoreCompat.System.Drawing.BarcodeReader();
            //using (var coreCompatImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(@"C:\Users\Xavier\Pictures\qrimage.png"))
            //{
            //    var coreCompatResult = coreCompatReader.Decode(coreCompatImage);
            //}

            //try
            //{
            //    Bitmap bitmap = new Bitmap(@"C:\Barcode\Faktura-5342.Pdf");
            //    BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryInverted = true };
            //    scanBarcode scan = new scanBarcode();

            //    Result result1 = reader.Decode(bitmap);
            //    string decoded = result1.ToString().Trim();

            //}
            //catch (Exception)
            //{

            //}

            //var image = (Bitmap)Image.FromFile(@"C:\Barcode\Faktura-5342.Pdf");
            //// do something with the result
            //using (image)
            //{
            //    // decode the barcode from the in memory bitmap
            //    var b = reader2.Decode(image);
            //    var barcodeResult = barcodeReader.decode(image);
            //    //source = new BitmapLuminanceSource(image);
            //    BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(image));
            //    Result result = new MultiFormatReader().decode(bitmap);
            //    if (result != null)
            //    {

            //    }
            //    else
            //    {

            //    }

            //}




            // detect and decode the barcode inside the bitmap
            //var result = reader.Decode((Bitmap)images.FirstOrDefault());
            //// do something with the result
            //if (result != null)
            //{
            //    txtDecoderType.Text = result.BarcodeFormat.ToString();
            //    txtDecoderContent.Text = result.Text;
            //}


        }
    }
}