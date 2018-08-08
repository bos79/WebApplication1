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
using System.Threading;
using System.Net;
using Newtonsoft.Json.Converters;
using System.Globalization;
using SweIdNum;
using SweIdNum.Core;


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
        public static List<string> PdfList;
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        public static string ReadFiles()
        {
            if (Program.FilePhth != null)
            {
                MovePdf(Program.FilePhth);
            }
            if(Program.count > 1)
            {
                Program.count = 1;
            }
            if (CombineList != null)
            {
                CombineList.Clear();
            }
            string PdfPath = @"C:\Pdf";
            PdfList = Directory.GetFiles(PdfPath, "*.pdf").ToList();
            
                foreach (string file in PdfList)
                {
                    if (Program.FilePhth != file)
                    {
                        Program.FilePhth = file;
                        return file;
                    }

                }
            return null;
        }
        //flytta filer
        public static void MovePdf(string filephth)
        {
            int i = 0;
            
            string sourceFile = filephth;
            string destinationFile = @"C:\AvlästaPdf\avläst.pdf";

            // To move a file or folder to a new location:
            System.IO.File.Move(sourceFile, destinationFile);

            // To move an entire directory. To programmatically modify or combine
            // path strings, use the System.IO.Path class.
            //System.IO.Directory.Move(@"C:\Users\Public\public\test\", @"C:\Users\Public\private");
        }
        
        public static List<string> ReadImages()
        {
            var pages = Program.NumberOfPagesPdf(Program.FilePhth);
            List<string> ImageList = new List<string>();
            string ImagePath = @"C:\Images\"+pages+"\\";
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
                //ränsar ut gammal ord
                WordList.Clear();
                List<string> CombineWordList = new List<string>();
                CombineWordList.Clear();
                var OrResult = JsonConvert.DeserializeObject<OcrResults>(contentString);
               // string Pdf = Program.ReadFiles();
                int NrOfPages = Program.NumberOfPagesPdf(Program.FilePhth);
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
                    await Program.ExtraktAndGetImages(Program.FilePhth, NrOfPages);
                }
                eInvoice invoice = new eInvoice
                {
                };
                var images = PdfImageExtractor.PageContainsImages(Program.FilePhth, NrOfPages);
                var cashList = getTheTotalAmount(Program.CombineList);
                
                getTheMaxValueAndCalculateMons(cashList , invoice);
                GetMons(Program.CombineList , invoice);
                GetOrgNumber(Program.CombineList , invoice);
                AddingDateAndOrcNr( Program.CombineList , invoice);
                await PostModelToController(invoice);
                //Program.DeleteFile(Program.FilePhth);
                  
                
                
                

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
        public static void AddingDateAndOrcNr(List<string> WordList , eInvoice invoice)
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
                                        invoice.Ocr = WordList[i];
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
                                        invoice.Ocr = WordList[i];
                                        break;
                                    }

                                }
                            }
                        }
                    }

                 }
                //jämför datum älsta är när fakturan ska betalas och tidigaste är fakturadatum
            var dateListLenght = DateList.Count();

            
            DateTime lastDate;
            for (i = 0; i < dateListLenght; i++)
            {
                j = i + 1;
                if(j > dateListLenght)
                {
                    j = dateListLenght;
                }
                int result = DateTime.Compare( DateTime.Parse( DateList[i]), DateTime.Parse(DateList[j]));

                //is earlier than
                if (result < 0)
                {
                    invoice.DueD = DateTime.Parse(DateList[j]).ToString();
                    invoice.InvoiceD = DateTime.Parse(DateList[i]).ToString();
                    
                    break;
                }
                //is the same time as
                else if (result == 0)
                {
                    lastDate = DateTime.Parse(DateList[j]);
                }
                //is later than
                else
                {

                    invoice.DueD = DateTime.Parse(DateList[i]).ToString();
                    invoice.InvoiceD = DateTime.Parse(DateList[j]).ToString();
                    //var date = DateTime.Parse(DateList[i]);
                    //var dateOnly = date.Date;
                    //var lastDate1 = DateTime.ParseExact(DateList[i], "yyyy-MM-dd", null);
                    //var FirstDate = DateTime.ParseExact(DateList[j], "yyyy-MM-dd", null);
                    //string json1 = JsonConvert.SerializeObject(dateOnly.Date);
                    //string json2 = JsonConvert.SerializeObject(FirstDate.Date);
                    //string json1 = lastDate1.ToUniversalTime().ToString("s") + "Z";
                    //string json2 = FirstDate.ToUniversalTime().ToString("s") + "Z";
                    //invoice.DueDate = DateTime.Parse(json1);
                    //invoice.InvoiceDate = DateTime.Parse(json2);


                    break;
                }
                    
            }
               
        }
        public static void GetMons(List<string> WordList , eInvoice invoice)
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
        //kollar om moms o max värdet stämmer om det är så skicka till db
        public static void getTheMaxValueAndCalculateMons(List<String> MoneyList, eInvoice eInvoice)
        {
            List<string> TotaltVärde = new List<string>();
            List<double> TotalMoms = new List<double>();
            double MomsTjugoFem = 1.25;
            double MomsTolv = 1.12;
            double MomsSex = 1.06;
            double max = Convert.ToDouble(MoneyList.OrderByDescending(v => Decimal.Parse(v)).FirstOrDefault());
            //Öres utjämning
            //var deci = max - Math.Truncate(max);
            //if (deci >= 0.5)
            //{
            //    max++;
            //}

            //25 Moms beräkning
            var sum = max / MomsTjugoFem;
            var resMoms25 = max - sum;
            //12 Moms beräkning
            var sum2 = max / MomsTolv;
            var resMoms12 = max - sum2;
            //6 Moms beräkning
            var sum3 = max / MomsSex;
            var resMoms6 = max - sum3;
            foreach (var item in MoneyList)
            {
                if(Math.Truncate(resMoms25) == Math.Truncate( Convert.ToDouble(item)))
                {
                    TotaltVärde.Add(max.ToString());
                    TotalMoms.Add(Math.Truncate(resMoms25));
                    eInvoice.Amount = max;
                    eInvoice.VatAmount = Math.Truncate(resMoms25);
                }
                else if(Math.Truncate(resMoms12) == Math.Truncate(Convert.ToDouble(item)))
                {
                    TotaltVärde.Add(max.ToString());
                    TotalMoms.Add(resMoms25);
                    eInvoice.Amount = max;
                    eInvoice.VatAmount = Math.Truncate(resMoms12);
                }
                else if (Math.Truncate(resMoms6) == Math.Truncate(Convert.ToDouble(item)))
                {
                    TotaltVärde.Add(max.ToString());
                    TotalMoms.Add(resMoms25);
                    eInvoice.Amount = max;
                    eInvoice.VatAmount = Math.Truncate(resMoms6);
                }
            }

        }
        public static void createANewModel()
        {
            
        }
        public static void addToModel(eInvoice eInvoice)
        {
            
        }
        public static async Task PostModelToController(eInvoice ListOfData)
        {
            //IsoDateTimeConverter iso = new IsoDateTimeConverter(){ DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            //JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            //{
            //    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            //};
            var jsonString = await Task.Run(() => JsonConvert.SerializeObject(ListOfData, Formatting.None, new JavaScriptDateTimeConverter()));
            //string javascriptJson = JsonConvert.SerializeObject(jsonString);
            var str = new StringContent(jsonString , Encoding.UTF8 , "application/json");
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.BaseAddress = new Uri("http://localhost:26695/");
                await httpClient.PostAsync("/eInvoices/TestPost", str);
            }
        }
       
        public static List<string> getTheTotalAmount(List<String> WordList)   
        {
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

                                bool containsNum1 = Regex.IsMatch(WordList[i - 1], @"^(^\d{1,3})$");
                                if (containsNum1)
                                {
                                    //kollar om föregående talet i listan innehåller 1-3 siffror
                                    foreach (Match match2 in rgx2.Matches(WordList[i - 1]))
                                    {
                                        if (match2.Success == true)
                                        {

                                            var sum = match2.Value + match.Value;
                                            bool containsNum2 = Regex.IsMatch(match2.Value, @"^(^\d{3})$");
                                            if (containsNum2)
                                            {
                                                //om nummret inehåller 3 siffror kolla om föregående tal är en match
                                                foreach (Match match3 in rgx2.Matches(WordList[i - 2]))
                                                {
                                                    if (match3.Success == true)
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
                                        else
                                        {
                                            PengarLista.Add(match2.Value);
                                            break;
                                        }


                                    }
                                }
                                else
                                {
                                    PengarLista.Add(match.Value);
                                    break;
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
        public static void GetOrgNumber(List<string> WordList , eInvoice invoice)
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
                            invoice.OrgNo = WordList[i];
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
                            invoice.OrgNo = WordList[i];
                        }
                    }
                }

            }
        }
        //kollar om nummret är ett orgnr
        public static bool checkIfItIsAOrgNr(string data)
        {
            List<int> sum = new List<int>();
            int tSum = 0;
            string numericOrg = new String(data.Where(Char.IsDigit).ToArray());
            for (int i = 0 ; i < numericOrg.Length; i++)
            {
                //om talet är jämt
                if(i%2 == 0)
                {
                     sum.Add( Convert.ToInt32(numericOrg[i].ToString()) * 2);
                }
                //eller udda
                else
                {
                    sum.Add( Convert.ToInt32(numericOrg[i].ToString()) * 1);
                }
                
            }
            
            foreach (var item in sum)
            {
                if (item >= 10)
                {
                    string tData = item.ToString();
                    tSum += Convert.ToInt32(tData[0].ToString()) + Convert.ToInt32(tData[1].ToString());
                }
                else
                {
                    tSum += Convert.ToInt32(item);
                }
            }
            int controlNr = 0;
            var stringtSum = tSum.ToString();
            //var shar1 = Convert.ToInt32(stringtSum[0].ToString());
            //var shar2 = Convert.ToInt32(stringtSum[1].ToString());
            //if (shar2 < 0)
            //{
            //    shar1++;
            //    shar1 *= 10;
            //    controlNr = shar1 - tSum;
            //}
            //else
            //{
            //    shar1 *= 10;
            //    controlNr = shar1 - tSum;
            //}
            //om det är delbart med 10 så är det ett org nr
            if (tSum % 10 == 0)
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
    



