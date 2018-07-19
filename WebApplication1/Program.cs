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
using Simplify.Windows.Forms;
using System.Windows.Forms.RibbonHelpers;
using System.Threading;

namespace WebApplication1
{
    public class Program
    {
        public static int count = 1;
        public static void Main(string[] args)
        {

            //PdfImageExtractor.AddFile();
            string PdfPath = @"C:\Pdf";
            //string fileName = "0004B9B7.pdf";
            string Pdf = ReadFiles();
            int NrOfPages = NumberOfPagesPdf(Pdf);
            ExtraktAndGetImages(Pdf, NrOfPages);
            // string Pdf = System.IO.Path.Combine(PdfPath, fileName);
           
            


            BuildWebHost(args).Run();



        }
        public static async Task ExtraktAndGetImages (string Pdf, int NrOfPages)
        {

            if (PdfImageExtractor.PageContainsImages(Pdf, NrOfPages) == true)
            {
                PdfImageExtractor.ExtractImage(Pdf);
                var Images = ReadImages();
                await BildLäs.Main2(Images.Where(m => m.Contains(Program.count.ToString())).FirstOrDefault());

            }
        }
        public static string FilePhth;
        public static List<string> CombineList;

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        public static string ReadFiles()
        {
            string PdfPath = @"C:\Pdf";
            foreach (string file in Directory.EnumerateFiles(PdfPath, "*.pdf"))
            {
                Program.FilePhth = file;
                return file;
              
                
            }
            return null;
        }
        public static List<string> ReadImages()
        {
            List<string> ImageList = new List<string>();
            string ImagePath = @"C:\Images";
            foreach (string file in Directory.EnumerateFiles(ImagePath, "*.jpg"))
            {
                ImageList.Add(file);
                


            }
            return ImageList;
        }
        public static int NumberOfPagesPdf(string pdf)
        {
            PdfReader pdfreader = new PdfReader(pdf);
            return pdfreader.NumberOfPages;
        }
        public static void DeleteFile(string pdf)
        {
            const int NumberOfRetries = 3;
            const int DelayOnRetry = 1000;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    System.IO.File.Delete(pdf);
                    break; // When done we can break loop
                }
                catch (IOException e) when (i <= NumberOfRetries)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    Thread.Sleep(DelayOnRetry);

                }
            }
        }
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
                List<string> WordList = new List<string>();
                List<string> CombineWordList = new List<string>();
                var OrResult = JsonConvert.DeserializeObject<OcrResults>(contentString);
                string Pdf = Program.ReadFiles();
                int NrOfPages = Program.NumberOfPagesPdf(Pdf);
                var result = StringBilders(OrResult);

                if (Program.CombineList != null)
                {
                    Program.CombineList = WordList.Concat(AddWordsToList(OrResult)).Concat(Program.CombineList).ToList();
                }
                else
                {
                    Program.CombineList = WordList.Concat(AddWordsToList(OrResult)).ToList();
                }
                //om det finns flera pdf sidor så körs det om och sparar det andra i en templista
                if (NrOfPages > Program.count)
                {
                   
                    Program.count++;
                    await Program.ExtraktAndGetImages(Pdf, NrOfPages);
                }
                getTheTotalAmount(Program.CombineList);
                GetMons(Program.CombineList);
                GetOrgNumber(Program.CombineList);
                SearchList("Förfallodatum:", Program.CombineList);
                Program.DeleteFile(Program.FilePhth);
                  
                
                
                var images = PdfImageExtractor.PageContainsImages(Pdf, NrOfPages);

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
        public static void SearchList(string SearchString, List<string> WordList)
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
                                        break;
                                    }
                                  
                                }
                            }
                            else if (WordList[i].Length > 9)
                            {
                                var h = WordList[i].Count() - 2;
                                for (int g = 0; g < WordList[i].Length; g++)
                                {
                                   
                                    g = h;
                                    var c = (int)Char.GetNumericValue(nr[g]); 
                                    var tioTal = c + 10;
                                    g++;
                                    var OcrNrLängd = tioTal;
                                    if (OcrNrLängd == WordList[i].Length) 
                                    {
                                        OrcList.Add(WordList[i]);
                                        g = WordList[i].Length;
                                        break;
                                    }

                                }
                            }
                        }
                    }

                 }
        }
        public static void GetMons(List<string> WordList)
        {
        int i;
        var lenght = WordList.Count();
        List<string> MonsList = new List<string>();
        List<string> NettoList = new List<string>();
        List<string> PengarLista = new List<string>();
        List<string> TotalList = new List<string>();
        string patternMons = @"^\d+(,\d{1,2})$";
        Regex rgx = new Regex(patternMons);

            for (i = 0; i < lenght; i++)
            {
                foreach (Match match in rgx.Matches(WordList[i]))
                {
                    if (match.Success == true)
                    {
                    PengarLista.Add(WordList[i]);

                    }
                }

            }
            
        }

        public static void getTheMaxValueAndCalculateMons(List<String> MoneyList)
        {

        }
        public static List<string> getTheTotalAmount(List<String> WordList)   
        {
            decimal number;
            int i;
            var lenght = WordList.Count();
            List<string> PengarLista = new List<string>();
            string patternCoins = @"^\d+(,\d{1,2})$";
            string patternHundraMedDecimal = @"^(^\d{1,7})+(,\d{1,2})$";
            string patternRenHundra = @"^(^\d{1,3})$";
            Regex rgx = new Regex(patternHundraMedDecimal);
            Regex rgx2 = new Regex(patternRenHundra);
            for (i = 0; i < lenght; i++)
            {
                //kollar om talet har från 1 till 7 siffror och är comma separerat med 2 decimaler
                foreach (Match match in rgx.Matches(WordList[i]))
                {
                    if (match.Success == true  )
                    {
                        
                        
                            bool containsNum = Regex.IsMatch(match.Value, @"^(^\d{3})+(,\d{1,2})$");
                            if (containsNum)
                            {
                               
                           
                                //kollar om föregående talet i listan innehåller 1-3 siffror
                                foreach (Match match2 in rgx2.Matches(WordList[i - 1]))
                                {
                                    if (match.Success == true)
                                    {

                                        var sum = match2.Value + match.Value;
                                        bool containsNum2 = Regex.IsMatch(match2.Value, @"^(^\d{3})$");
                                        if (containsNum2)
                                        {
                                            //om nummret inehåller 3 siffror kolla om föregående tal är en match
                                            foreach (Match match3 in rgx2.Matches(WordList[i - 2]))
                                            {
                                                if (match.Success == true)
                                                {
                                                    var sum1 = match3.Value + match2.Value + match.Value;
                                                    bool containsNum3 = Regex.IsMatch(match3.Value, @"^(^\d{3})$");
                                                    if (containsNum3)
                                                    {
                                                        foreach (Match match4 in rgx2.Matches(WordList[i - 3]))
                                                        {
                                                            if (match.Success == true)
                                                            {
                                                                var sum2 = match4.Value + match3.Value + match2.Value + match.Value;
                                                                PengarLista.Add(sum2);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        PengarLista.Add(sum1);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            PengarLista.Add(sum);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                PengarLista.Add(match.Value);
                            }
                       
                    }
                }
            }

            return PengarLista;
        }
        public static void GetOrgNumber(List<string> WordList)
        {
            int i;
            var lenght = WordList.Count();
            List<string> OrgNrList = new List<string>();
            string patternOrgNr = @" ^[0-9]{10}$";
            string patternOrgNrWhitALineInIt = @"^[0-9]{6}-[0-9]{4}$";
            Regex rgx = new Regex(patternOrgNr);
            Regex rgx2 = new Regex(patternOrgNrWhitALineInIt);

            for (i = 0; i < lenght; i++)
            {
                foreach (Match match in rgx.Matches(WordList[i]) )
                {
                    if (match.Success == true)
                    {
                        if (true == checkIfItIsAOrgNr(WordList[i]))
                        {
                            OrgNrList.Add(WordList[i]);
                        }
                    }
                }
                foreach (Match match in  rgx2.Matches(WordList[i]))
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
            string numericOrg = new String(data.Where(Char.IsDigit).ToArray());
            for (int i = numericOrg.Length - 1; i >= 0; i--)
            {
                if (odd == true)
                {
                    int tSum = Convert.ToInt32(numericOrg[i].ToString()) * 2;
                    if (tSum >= 10)
                    {
                        string tData = tSum.ToString();
                        tSum = Convert.ToInt32(tData[0].ToString()) + Convert.ToInt32(tData[1].ToString());
                    }
                    sum += tSum;
                }
                else
                    sum += Convert.ToInt32(numericOrg[i].ToString());
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
    



