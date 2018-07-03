using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Ghostscript.NET;
using Ghostscript.NET.Interpreter;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace WebApplication1
{
    public class Program
    {

        public static void Main(string[] args)
        {

            PdfImageExtractor.AddFile();
            string PdfPath = @"C:\Users\ERIP\Downloads";
            string fileName = "0004B9B7.pdf";
            string Pdf = System.IO.Path.Combine(PdfPath, fileName);
            var images = PdfImageExtractor.ExtractImages(Pdf);
         
            //BildLäs.SaveFile(images);
           // var data = BildLäs.Main2(Pdf);
            BuildWebHost(args).Run();



        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

    }

    
    public class BildLäs
    {
        public String JsonFile { get; set; }
        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "ea6cd28f14ce464ca99359e08ffe9d80";

        // You must use the same region in your REST call as you used to
        // get your subscription keys. For example, if you got your
        // subscription keys from westus, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the westcentralus region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriBase =
            "https://westeurope.api.cognitive.microsoft.com/vision/v1.0/ocr";
        public static async Task<String> Main2(String Pdf)
        {
           

            // Get the path and filename to process from the user.
            //Console.WriteLine("Optical Character Recognition:");
            //Console.Write("Enter the path to an image with text you wish to read: ");
            string imageFilePath = Pdf;

            if (File.Exists(imageFilePath))
            {
                // Make the REST API call.
                Console.WriteLine("\nWait a moment for the results to appear.\n");
                string jsonData = await MakeOCRRequest(imageFilePath);

                BildLäs.ReturnJson(jsonData);

                return jsonData;

            }
            else
            {
                Console.WriteLine("\nInvalid file path");
                return null;
            }



        }
        /// <summary>
        /// Gets the text visible in the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        private static async Task<string> MakeOCRRequest(string imageFilePath)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.
                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.

                Console.WriteLine("\nResponse:\n\n{0}\n",
                JToken.Parse(contentString).ToString());
                return contentString;
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
            }
        }
        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
        public static string ReturnJson(string j)
        {
            // serialize JSON to a string and then write string to a file
            File.WriteAllText(@"C:\Users\ERIP\Downloads\JsonFile.json", j);

            using (StreamWriter file = File.CreateText(@"C:\Users\ERIP\Downloads\JsonFile.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, j);
            }
            ReadToObject(j);
            return j;
        }
        public static Models.eInvoice ReadToObject(string json)
        {
            Models.eInvoice deserializedUser = new Models.eInvoice();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(deserializedUser.GetType());
            deserializedUser = ser.ReadObject(ms) as Models.eInvoice;
            ms.Close();
           
           

            return deserializedUser;
        }
    }
 }
    



