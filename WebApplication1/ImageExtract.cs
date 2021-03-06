﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO.IsolatedStorage;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text;
using System.Drawing.Imaging;
using System.Reflection;
using System.Threading;
using System.IO;

namespace WebApplication1
{

    public static class PdfImageExtractor
    {
        public static int increment = 0;

        public static string CurrentFilename;
        #region Methods

        #region Public Methods
        public static void AddFile()
        {
            string PdfPath = @"C:\Users\ERIP\Downloads";
            string fileName = "0004B9B7.pdf";
            string Pdf = System.IO.Path.Combine(PdfPath, fileName);
            PageContainsImages(Pdf, 1);
        }
        




    /// <summary>Checks whether a specified page of a PDF file contains images.</summary>
    /// <returns>True if the page contains at least one image; false otherwise.</returns>
    public static bool PageContainsImages(string filename, int pageNumber)
            {
            CurrentFilename = filename;
            using (var reader = new PdfReader(filename))
            {
                
                var parser = new PdfReaderContentParser(reader);
                ImageRenderListener listener = null;

                parser.ProcessContent(pageNumber, (listener = new ImageRenderListener()));
                return listener.Images.Count > 0;
            }
            
            

        }

            /// <summary>Extracts all images (of types that iTextSharp knows how to decode) from a PDF file.</summary>
            public static Dictionary<string, System.Drawing.Image> ExtractImage(string filename)
            {
                var images = new Dictionary<string, System.Drawing.Image>();

                using (var reader = new PdfReader(filename))
                {
                    List<string> textList = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    var parser = new PdfReaderContentParser(reader);
                    ImageRenderListener listener = null;
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    //läser in texten på en pdf
                    for (var i = 1; i <= reader.NumberOfPages; i++)
                    {
                         parser.ProcessContent(i, (listener = new ImageRenderListener()));
                         var text = PdfTextExtractor.GetTextFromPage(reader, i , strategy);
                         if (!string.IsNullOrWhiteSpace(text))
                         {
                              string[] lines = text.Split('\n',' ');
                              foreach (string line in lines)
                              {
                                    if (line != "")
                                    {
                                         textList.Add(line);
                                    }
                                   
                                 
                              }
                         }
                         Program.addCombineLists(textList);
                         var index = 1;
 
                        if (listener.Images.Count > 0)
                        {
                                Console.WriteLine("Found {0} images on page {1}.", listener.Images.Count, i);

                                foreach (var pair in listener.Images)
                                {
                                    images.Add(string.Format("{0}_Page_{1}_Image_{2}.{3}",
                                        System.IO.Path.GetFileNameWithoutExtension(filename), i.ToString("D4"), index.ToString("D4"), pair.Value), pair.Key);
                                    index++;
                                }
                        }
                    }
                 
                 return images;
                }
            }

            /// <summary>Extracts all images (of types that iTextSharp knows how to decode) 
            /// from a specified page of a PDF file.</summary>
            /// <returns>Returns a generic <see cref="Dictionary&lt;string, System.Drawing.Image&gt;"/>, 
            /// where the key is a suggested file name, in the format: PDF filename without extension, 
            /// page number and image index in the page.</returns>
            public static Dictionary<string, System.Drawing.Image> ExtractImages(string filename, int pageNumber)
            {
                Dictionary<string, System.Drawing.Image> images = new Dictionary<string, System.Drawing.Image>();
                PdfReader reader = new PdfReader(filename);
                PdfReaderContentParser parser = new PdfReaderContentParser(reader);
                ImageRenderListener listener = null;
                
                parser.ProcessContent(pageNumber, (listener = new ImageRenderListener()));
                int index = 1;

                if (listener.Images.Count > 0)
                {
                    Console.WriteLine("Found {0} images on page {1}.", listener.Images.Count, pageNumber);

                    foreach (KeyValuePair<System.Drawing.Image, string> pair in listener.Images)
                    {
                        images.Add(string.Format("{0}_Page_{1}_Image_{2}{3}",
                            System.IO.Path.GetFileNameWithoutExtension(filename), pageNumber.ToString("D4"), index.ToString("D4"), pair.Value), pair.Key);
                        index++;
                    }
                }
            
            return images;
            }

            #endregion Public Methods

            #endregion Methods
        }

        internal class ImageRenderListener : IRenderListener
        {
            #region Fields

            Dictionary<System.Drawing.Image, string> images = new Dictionary<System.Drawing.Image, string>();

            #endregion Fields

            #region Properties

            public Dictionary<System.Drawing.Image, string> Images
            {
                get { return images; }
            }

            #endregion Properties

            #region Methods

            #region Public Methods

            public void BeginTextBlock() { }

            public void EndTextBlock() { }

            
            public void RenderImage(ImageRenderInfo renderInfo)
            {
                PdfImageObject image = renderInfo.GetImage();
                
            
                var v = PdfName.FILTER;
                
                //PdfArray array = new PdfArray();
                //array.Add(PdfName.FLATEDECODE);
                //array.Add(PdfName.DCTDECODE);
                //imgStream.put(PdfName.FILTER, array);
                //PdfName filter = (PdfName)image.Get(PdfName.FILTER);
                PdfName filter = (PdfName)image.Get(PdfName.FIRST );
                int width = Convert.ToInt32(image.Get(PdfName.WIDTH).ToString());
                int bitsPerComponent = Convert.ToInt32(image.Get(PdfName.BITSPERCOMPONENT).ToString());
                string subtype = image.Get(PdfName.SUBTYPE).ToString();
                int height = Convert.ToInt32(image.Get(PdfName.HEIGHT).ToString());
                int length = Convert.ToInt32(image.Get(PdfName.LENGTH).ToString());
                string colorSpace = image.Get(PdfName.COLORSPACE).ToString();

            /* It appears to be safe to assume that when filter == null, PdfImageObject 
             * does not know how to decode the image to a System.Drawing.Image.
             * 
             * Uncomment the code above to verify, but when I've seen this happen, 
             * width, height and bits per component all equal zero as well. */
            //if (filter != null)
            //{
                    Image drawingImage = image.GetDrawingImage();

            string extension = PdfImageObject.ImageBytesType.JPG.FileExtension;

            //if (filter == PdfName.DCTDECODE)
            //{
            //    extension += PdfImageObject.ImageBytesType.JPG.FileExtension;
            //}
            //else if (filter == PdfName.JPXDECODE)
            //{
            //    extension += PdfImageObject.ImageBytesType.JP2.FileExtension;
            //}
            //else if (filter == PdfName.FLATEDECODE)
            //{
            //    extension += PdfImageObject.ImageBytesType.PNG.FileExtension;
            //}
            //else if (filter == PdfName.LZWDECODE)
            //{
            //    extension += PdfImageObject.ImageBytesType.CCITT.FileExtension;
            //}

            /* Rather than struggle with the image stream and try to figure out how to handle 
             * BitMapData scan lines in various formats (like virtually every sample I've found 
             * online), use the PdfImageObject.GetDrawingImage() method, which does the work for us. */
            try
            {

                var pages = Program.NumberOfPagesPdf(Program.FilePhth);
                this.Images.Add(drawingImage, extension);
                string filename = @"C:\Images\" + pages + "\\";
                bool exists = System.IO.Directory.Exists(filename);
                PdfImageExtractor.increment++;
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(filename);
                }
                
                if (PdfImageExtractor.increment <= pages)
                {
                    string fullName2 = filename + "im" + PdfImageExtractor.increment + ".JPG";
                    byte[] byteArray = Encoding.UTF8.GetBytes(fullName2);
                    MemoryStream stream = new MemoryStream();
                    drawingImage.Save(stream, drawingImage.RawFormat);
                    drawingImage.Save(fullName2, ImageFormat.Gif);
                    if ( Program.invoice.images1.Any(m => m.ImageName == fullName2))
                    {
                        
                    }
                    else
                    {
                        //sparar bilden till modellen
                        Program.invoice.images1.Add(new Models.Images()
                        {

                            ImageName = fullName2,

                            ImageData = stream.ToArray()
                        });
                    }
                    
                   
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
                    
  
        }


        List<string> textlist = new List<string>();
        public void RenderText(TextRenderInfo renderInfo)
        {
        
        }

            #endregion Public Methods

            #endregion Methods
        }
}

