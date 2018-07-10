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
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Microsoft.ProjectOxford.Vision.Contract;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

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
         
 
            BuildWebHost(args).Run();



        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

    }

    
    public class BildLäs
    {
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

            if (System.IO.File.Exists(imageFilePath))
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
                string contentString = await   response.Content.ReadAsStringAsync();

                // Display the JSON response.
                var OrResult = JsonConvert.DeserializeObject<OcrResults>(contentString);
                var result = StringBilders(OrResult);
                var list = AddWordsToList(OrResult);
                GetMons(list);
                GetOrgNumber(list);
                SearchList("Förfallodatum:", list);
                



                return  result;
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
           System.IO.File.WriteAllText(@"C:\Users\ERIP\Downloads\JsonFile.json", j);

            using (StreamWriter file = System.IO.File.CreateText(@"wwwroot\Ajax\JsonFile.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, j);
            }
            ReadToObject(j);
            return j;
        }
        public static List<string> SearchList(string SearchString, List<string> WordList)
        {
            using (var db = new WebApplication1Context())
            {
                var lenght = WordList.Count;
                List<string> DateList = new List<string>();
                List<string> OrcList = new List<string>();
                int i ;
                int j = 0;
                string pattern = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))";
                string numbersPattern = @"^[0-9]*$";
                string patternOrcNr = @"^[0-9]{"+j+"}$";
                Regex rgx = new Regex(pattern);
                Regex rgx2 = new Regex(numbersPattern);
                // var m = WordList.Where(x => x.Contains(string.Format("1900-00-00"))).FirstOrDefault().ToString();
                for (i = 0; i < lenght; i++)
                {
                    foreach (Match match in rgx.Matches(WordList[i]))
                    {
                        if (match.Success == true)
                        {
                            DateList.Add(WordList[i]);
                        }
                    }
                    //Kollar längden på nummren och plockaq ut längden på ocr-nummret
                    foreach (Match match in rgx2.Matches(WordList[i]))
                    {
                        if (match.Success == true && WordList[i].Length > 4 )
                        {
                            var nr = WordList[i];
                            if (WordList[i].Length <= 9)
                            {
                                var h = WordList[i].Count()-2;
                                for (int g = 0; g < WordList[i].Length ; g++)
                                {

                                    if((int)Char.GetNumericValue(nr[g]) == (WordList[i].Length-1) && (int)Char.GetNumericValue(nr[g]) == WordList[i].Length)
                                    {
                                        OrcList.Add(WordList[i]);
                                    }
                                  
                                }
                            }
                            else if (WordList[i].Length > 9)
                            {
                                var h = WordList[i].Count() - 3;
                                for (int g = 0; g < WordList[i].Length; g++)
                                {
                                   
                                    g = h;
                                    var c = (int)Char.GetNumericValue(nr[g]); 
                                    var tioTal = c * 10;
                                    g++;
                                    var OcrNrLängd = (int)Char.GetNumericValue(nr[g]) + tioTal;
                                    if (OcrNrLängd == WordList[i].Length) 
                                    {
                                        OrcList.Add(WordList[i]);
                                        g = WordList[i].Length;
                                    }

                                }
                            }
                        }
                    }


                }

                return DateList;
            }
        }
        public static void GetMons(List<string> WordList)
        {
            int i;
            var lenght = WordList.Count();
            List<string> MonsList = new List<string>();
            string patternMons = @"^\d+(,\d{1,2})$";
            Regex rgx = new Regex(patternMons);

            for (i = 0; i < lenght; i++)
            {
                foreach (Match match in rgx.Matches(WordList[i]))
                {
                    if (match.Success == true)
                    {
                        var v = i;
                        if (WordList[v-1] == "Momsbelopp" || WordList[v-2] == "Momsbelopp" || WordList[v - 3] == "Momsbelopp")
                        {
                            MonsList.Add(WordList[i]);
                        }
                        
                    }
                }

            }
        }
        public static void GetOrgNumber(List<string> WordList)
        {
            int i;
            var lenght = WordList.Count();
            List<string> OrgNrList = new List<string>();
            string patternOrgNr = @"^[0-9]{10}$";
            Regex rgx = new Regex(patternOrgNr);

            for (i = 0; i < lenght; i++)
            {
                foreach (Match match in rgx.Matches(WordList[i]))
                {
                    if (match.Success == true)
                    {
                        if (true == checkIfItIsAOrgNr(WordList[i]))
                        {
                            OrgNrList.Add(WordList[i]);
                        }
                    }
                }

            }
        }
        public static bool checkIfItIsAOrgNr(string data)
        {
            int sum = 0;
            bool odd = true;
            for (int i = data.Length - 1; i >= 0; i--)
            {
                if (odd == true)
                {
                    int tSum = Convert.ToInt32(data[i].ToString()) * 2;
                    if (tSum >= 10)
                    {
                        string tData = tSum.ToString();
                        tSum = Convert.ToInt32(tData[0].ToString()) + Convert.ToInt32(tData[1].ToString());
                    }
                    sum += tSum;
                }
                else
                    sum += Convert.ToInt32(data[i].ToString());
                odd = !odd;
            }
            if (sum % 10 == 0)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
        public static List<string> AddWordsToList(OcrResults results)
        {
            List<string> WordList = new List<string>();
            

            if (results != null && results.Regions != null)
            {
                foreach (var item in results.Regions)
                {
                    foreach (var line in item.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                           
                            WordList.Add(word.Text);
                           
                        }
                        
                    }
                   
                }
            }
            return WordList;
        }
        public static string StringBilders(OcrResults results)
        {
                List<string> WordList = new List<string>();
                StringBuilder stringBuilder = new StringBuilder();

                if (results != null && results.Regions != null)
                {
                    foreach (var item in results.Regions)
                    {
                        foreach (var line in item.Lines)
                        {
                            foreach (var word in line.Words)
                            {
                                stringBuilder.Append(word.Text);
                                WordList.Add(word.Text);
                                stringBuilder.Append(" ");
                            }
                            stringBuilder.AppendLine();
                        }
                        stringBuilder.AppendLine();
                    }
                }
                return stringBuilder.ToString();
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
    



