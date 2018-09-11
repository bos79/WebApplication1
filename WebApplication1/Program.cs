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
using System.Diagnostics;

namespace WebApplication1
{
    public class Program
    {
        public static eInvoice invoice = new eInvoice();
        public static int count = 1;
        public static string pdfName;
        public static bool websiteRunsWhenFalse = true;
        public static void Main(string[] args)
        {
            pdfName = ReadFiles();
            if (websiteRunsWhenFalse != false)
            {
                
                int NrOfPages = NumberOfPagesPdf(pdfName);
                try
                {
                    var v = ExtraktAndGetImages(pdfName, NrOfPages);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }

            BuildWebHost(args).Run();



        }
        public static async Task ExtraktAndGetImages(string Pdf, int NrOfPages)
        {

            if (PdfImageExtractor.PageContainsImages(Pdf, NrOfPages) == true)
            {

                var image = PdfImageExtractor.ExtractImage(Pdf);


                var Images = ReadImages(invoice);
                await BildLäs.Main2(Images.Where(m => m.Contains(Program.count.ToString())).FirstOrDefault());

            }
        }
        public static string FilePhth;
        public static string FileName;
        public static List<string> CombineList = new List<string>();
        public static List<string> tempCashList = new List<string>();

        public static void addCombineLists(List<string> lista)
        {

            CombineList = lista.Concat(CombineList).ToList();

        }

        public static List<string> PdfList;
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        public static string ReadFiles()
        {
            Program.invoice = new eInvoice();
            PdfImageExtractor.increment = 0;
            string path = @"C:\AvlästaPdf\";
            if (Program.FilePhth != null)
            {
                MovePdf(Program.FilePhth, path+FileName);
            }
            if (Program.count > 1)
            {
                Program.count = 1;
            }
            if (Program.CombineList != null)
            {
                Program.CombineList.Clear();
            }
            string PdfPath = @"C:\Pdf";
            PdfList = Directory.GetFiles(PdfPath, "*.pdf").ToList();
            if(PdfList.Count == 0)
            {
                websiteRunsWhenFalse = false;
            }

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
        // avlästa pdfer får ett nr
        public static int i;
        public static string genereraPdfNamn()
        {
            i++;

            string destinationFile = @"C:\AvlästaPdf\avläst" + i + ".pdf";
            bool exists = System.IO.File.Exists(destinationFile);

            while (exists == true)
            {
                i++;
                destinationFile = @"C:\AvlästaPdf\avläst" + i + ".pdf";
                exists = System.IO.File.Exists(destinationFile);
            }
            destinationFile= @"avläst" + i + ".pdf";
            return destinationFile;
        }
        public static void SaveImage(string fileName)
        {
            var pages = Program.NumberOfPagesPdf(Program.FilePhth);
            string filename = @"C:\Images\" + pages + "\\";
            bool exists = System.IO.Directory.Exists(filename);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(filename);
            }
            string fullName = filename + fileName;
            System.IO.File.Create(fullName);
        }
        //flytta filer
        public static void MovePdf(string filephth, string fileName)
        {
            const int NumberOfRetries = 20;
            const int DelayOnRetry = 500;

            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {
                    // To move a file or folder to a new location:
                    System.IO.File.Move(filephth, fileName);
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
        public static int numberOfPagesCount()
        {
            int count = 0;
            var pages = Program.NumberOfPagesPdf(Program.FilePhth);
            string ImagePath = @"C:\Images\" + pages + "\\";
            foreach (string file in Directory.EnumerateFiles(ImagePath, "*.jpg"))
            {
                count++;
            }
            return count;
        }
        public static List<string> ImageNameList = new List<string>();
        //läser bilder och retunerar alla bilder
        public static List<string> ReadImages( eInvoice eInvoices)
        {
            
            var pages = Program.NumberOfPagesPdf(Program.FilePhth);
            List<string> ImageList = new List<string>();
            var allImages = new List<Images>();
            string ImagePath = @"C:\Images\" + pages + "\\";
            var imageList = Directory.GetFileSystemEntries(ImagePath, "*.jpg").ToList();
            foreach(var file in imageList)
            {
                ImageList.Add(file);
 
                if (eInvoices.images1.Count == 0)
                {
                    Program.ImageNameList.Add(file.ToString());
 
                }
                else
                {
                    if (ImageNameList != null)
                    {
                        bool koll = ImageNameList.Any(stringToCheck => stringToCheck.Contains(file.ToString()));
                        if (koll == false) 
                        {
                       
                            ImageNameList.Add(file.ToString());
                        }
                    }
                }
 
                
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
            //Optical Character Recognition;

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
        public static async Task<string> MakeOCRRequest(string imageFilePath)
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


                List<string> WordList = new List<string>();
                //ränsar ut gammal ord
                WordList.Clear();
                List<string> CombineWordList = new List<string>();
                CombineWordList.Clear();
                var OrResult = JsonConvert.DeserializeObject<OcrResults>(contentString);
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
                //eInvoice invoice = new eInvoice
                {
                };
                //AddingTotalSum(Program.CombineList, Program.invoice);
                var cashList = getTheTotalAmount(Program.CombineList);
                
                AddingFakturaNr(Program.CombineList,Program.invoice);
                if (cashList.Count != 0)
                {
                    Program.tempCashList = cashList.ToList();
                    getTheMaxValueAndCalculateMons(cashList, Program.invoice);
                }
                GetMons(Program.CombineList, Program.invoice);
                orgNrMatchning(Program.CombineList, Program.invoice);
                GetOrgNumber(Program.CombineList, Program.invoice);
                AddingDateAndOrcNr(Program.CombineList, Program.invoice);
                Program.FileName = Program.genereraPdfNamn();
                Program.invoice.pdfPaths = Program.FileName;
                kollaAlltData(Program.invoice);
                //await saveToDb(Program.invoice);
                await PostModelToController(Program.invoice);






                return result;
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
        public static void AddingFakturaNr(List<string> WordList, eInvoice invoice)
        {
            var lenght = WordList.Count;
            string numbersPattern = @"^([0-9]+[a-öA-Ö]+|[a-öA-Ö]+[0-9]+|[0-9])[0-9a-öA-Ö]*$";
            Regex rgx = new Regex(numbersPattern);
            for (int i = 0; i < lenght; i++)
            {

                if (WordList[i] == "Faktura" && WordList[i + 1] == "nr:" || WordList[i] == "Faktura/Avi" && WordList[i++] == "nummer" || WordList[i] == "Fåktura" && WordList[i++] == "nr:")
                {
                    foreach (Match match in rgx.Matches(WordList[i + 2]))
                    {
                        if (match.Success == true)
                        {
                            invoice.InvoiceNo = WordList[i + 2];
                            break;
                        }
                    }
                }
                if (WordList[i] == "Fakturanr" || WordList[i] == "Fakturanummer" || WordList[i] == "Invoice" || WordList[i] == "Fakturanummer/OCR")
                {
                    foreach (Match match in rgx.Matches(WordList[i + 1]))
                    {
                        if (match.Success == true)
                        {
                            invoice.InvoiceNo = WordList[i + 1];
                            break;
                        }
                    }

                }
            }
        }
        public static void AddingTotalSum(List<string> WordList, eInvoice invoice)
        {
            var lenght = WordList.Count;
            string numbersPattern = @"(\d{1,7})+(,\d{1,2})|(\d{1,7})+([\.]\d{1,2})|(\d{1,7})";
            Regex rgx = new Regex(numbersPattern);
            for (int i = 0; i < lenght; i++)
            {
                
                if (WordList[i] == "Summa" && WordList[i + 1] == "att" && WordList[i + 2] == "betala" || WordList[i] == "Summa" && WordList[i + 1] == "att" && WordList[i + 2] == "betala:" || WordList[i] == "Totalt" && WordList[i + 1] == "att" && WordList[i + 2] == "betala")
                {
                    
                    for (int g =i ; g < lenght; g++)
                    {
                        foreach (Match match in rgx.Matches(WordList[g]))
                        {
                            if (match.Success == true)
                            {
                                invoice.Amount = double.Parse(WordList[g]);
                                break;
                            }
                        }
                    }
                }
                if (WordList[i] == "Att" && WordList[i + 1] == "betala" || WordList[i] == "Total" && WordList[i + 1] == "summa" || WordList[i] == "Total" && WordList[i + 1] == "amount")
                {
                    for (int g = i; g < lenght; g++)
                    {
                        foreach (Match match in rgx.Matches(WordList[g]))
                        {
                            if (match.Success == true)
                            {
                                invoice.Amount = double.Parse(WordList[g]);
                                break;
                            }
                        }
                    }
                }
                if (WordList[i] == "Summa" || WordList[i] == "Fakturanummer" || WordList[i] == "Invoice")
                {
                    for (int g = i; g < lenght; g++)
                    {
                        foreach (Match match in rgx.Matches(WordList[g]))
                        {
                            if (match.Success == true)
                            {
                                invoice.Amount = double.Parse(WordList[g]);
                                break;
                            }
                        }
                    }

                }
            }
        }
        public static DateTime InvoceDate;
        public static DateTime DuDate;
        public static void AddingDateAndOrcNr(List<string> WordList, eInvoice invoice)
        {

            var lenght = WordList.Count;
            List<string> DateList = new List<string>();
            List<string> OrcList = new List<string>();
            int i;
            int j = 0;
            //pattern för att matcha alla sorters datum
            string datePattern = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[123]\d|1[01])|((0[1-9]|[123]\d|1[01]|\S|[12]\d{3})[\.](0[1-9]|1[0-2]|\S)[\.]([12]\d{3}|0[0-9]|[123]\d|1[0]))|((0?[13578]|10|12)(-|\/)(([1-9])|(0[1-9])|([123])([0-9]?)|(1[0123]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))|(0?[2469]|11)(-|\/)(([1-9])|(0[1-9])|([123])([0-9]?)|(3[0]?))(-|\/)((19)([2-9])(\d{1})|(20)([01])(\d{1})|([8901])(\d{1}))))";
            string numbersPattern = @"^[0-9]*$";
            string patternOrcNr = @"^[0-9]{" + j + "}$";
            string patternCuttDate = @"^ (\d{1}-(0[1 - 9] | 1[0 - 2]) - (0[1 - 9] |[12]\d | 1[01]))$";
            bool rättDatum = false;
            Regex rgx = new Regex(datePattern);
            Regex rgx2 = new Regex(numbersPattern);
            Regex rgxCuttDate = new Regex(patternCuttDate);
            for (i = 0; i < lenght; i++)
            {

                if (WordList[i] == "Faktura" && WordList[i + 1] == "datum" || WordList[i] == "Fakturadatum" || WordList[i] == "Invocedate" || WordList[i] == "Fakturadatum:" || WordList[i] == "Datum")
                {


                    for (int g = i; g < lenght; g++)
                    {
                        //om datumet är delat
                        bool containsNum1 = Regex.IsMatch(WordList[g], @"^(\d{1}-(0[1 - 9] | 1[0 - 2]) - (0[1 - 9] |[12]\d | 1[01]))$");
                        if (containsNum1)
                        {
                            var heltDatum = WordList[g - 1] + WordList[g];
                            bool heltDatumMatch = Regex.IsMatch(heltDatum, datePattern);
                            if (heltDatumMatch)
                            {
                                InvoceDate = DateTime.Parse(WordList[g]);
                                DateList.Add(WordList[g]);
                                g = lenght;
                                break;
                            }
                        }
                        foreach (Match match in rgx.Matches(WordList[g]))
                        {
                            if (match.Success == true)
                            {
                                 InvoceDate = DateTime.Parse(WordList[g]);
                                 DateList.Add(WordList[g]);
                                 g = lenght;
                                 break;
                            }
                        }
                    }
                }
            }
            int count = 0;
            for (i = 0; i < lenght; i++)
            {
                if (WordList[i] == "Oss" && WordList[i + 1] == "tillhanda" && WordList[i + 2] == "senast" || WordList[i] == "Oss" && WordList[i + 1] == "tillhanda" || WordList[i] == "Betalnings" && WordList[i + 1] == "dag" || WordList[i] == "Dudate" || WordList[i] == "Förfallodatum" || WordList[i] == "Förfallet" || WordList[i] == "Förfallodag" || WordList[i] == "Förfallodatum:")
                {
                    
                        for (int g = i; g < lenght; g++)
                        {
                            bool containsNum1 = Regex.IsMatch(WordList[g], @"^(\d{ 1}-(0[1 - 9] | 1[0 - 2]) - (0[1 - 9] |[12]\d | 1[01]))$");
                            if (containsNum1)
                            {
                                var heltDatum = WordList[g - 1] + WordList[g];
                                bool heltDatumMatch = Regex.IsMatch(heltDatum, datePattern);
                                if (heltDatumMatch)
                                {
                                    DuDate = DateTime.Parse(heltDatum);
                                    DateList.Add(heltDatum);
                                    rättDatum = compareDate(DuDate, InvoceDate, invoice);
                                    if (rättDatum == true)
                                    {
                                        g = lenght;
                                        break;
                                    }
                                }
                            }
                            foreach (Match match in rgx.Matches(WordList[g]))
                            {
                                if (match.Success == true)
                                {
                                    DuDate = DateTime.Parse(WordList[g]);
                                    DateList.Add(WordList[g]);
                                    rättDatum = compareDate(DuDate, InvoceDate, invoice);
                                    if (rättDatum == true)
                                    {
                                        g = lenght;
                                        break;
                                    }
                                    
                                   
                                }
                            }
                            if (rättDatum == false && g == lenght - 1)
                            {
                                count++;
                                g = 1;
                                if (count <= 2)
                                {
                                    break;
                                }
                            }
                        }
                    
                }
            }
            for (i = 0; i < lenght; i++)
            {
                //Kollar längden på nummren och plockaq ut längden på ocr-nummret
                foreach (Match match in rgx2.Matches(WordList[i]))
                {
                    if (match.Success == true && WordList[i].Length > 4)
                    {
                        var nr = WordList[i];
                        if (WordList[i].Length <= 9)
                        {
                            var h = WordList[i].Count() - 2;

                            var chValue = (int)Char.GetNumericValue(nr[h]);
                            if (chValue == (WordList[i].Length))
                            {
                                if (true == checkIfItIsAOrgNr(WordList[i]))
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
        //var dateListLenght = DateList.Count();



        //for (i = 0; i < dateListLenght; i++)
        //{
        //    j = i + 1;
        //    if(j > dateListLenght)
        //    {
        //        j = dateListLenght;
        //    }
        //    int result = DateTime.Compare( DateTime.Parse( DateList[i]), DateTime.Parse(DateList[j]));

        //    //is earlier than
        //    if (result < 0)
        //    {
        //        invoice.DueD = DateTime.Parse(DateList[j]).ToString();
        //        invoice.InvoiceD = DateTime.Parse(DateList[i]).ToString();

        //        break;
        //    }
        //    //is the same time as
        //    else if (result == 0)
        //    {
        //        InvoceDate = DateTime.Parse(DateList[j]);
        //    }
        //    //is later than
        //    else
        //    {

        //        invoice.DueD = DateTime.Parse(DateList[i]).ToString();
        //        invoice.InvoiceD = DateTime.Parse(DateList[j]).ToString();
        //        //var date = DateTime.Parse(DateList[i]);
        //        //var dateOnly = date.Date;
        //        //var lastDate1 = DateTime.ParseExact(DateList[i], "yyyy-MM-dd", null);
        //        //var FirstDate = DateTime.ParseExact(DateList[j], "yyyy-MM-dd", null);
        //        //string json1 = JsonConvert.SerializeObject(dateOnly.Date);
        //        //string json2 = JsonConvert.SerializeObject(FirstDate.Date);
        //        //string json1 = lastDate1.ToUniversalTime().ToString("s") + "Z";
        //        //string json2 = FirstDate.ToUniversalTime().ToString("s") + "Z";
        //        //invoice.DueDate = DateTime.Parse(json1);
        //        //invoice.InvoiceDate = DateTime.Parse(json2);


        //        break;
        //    }

        //}

    
        }
        public static bool compareDate(DateTime duDate , DateTime invoceDate, eInvoice invoice)
        {
            int result = DateTime.Compare((DuDate), (InvoceDate));
            //is earlier than
            if (result < 0)
            {
                invoice.DueD = InvoceDate.ToString();
                invoice.InvoiceD = DuDate.ToString();

                
                
                return true;
            }
            //is the same time as
            else if (result == 0)
            {
                return false;
            }
            //is later than
            else
            {

                invoice.DueD = DuDate.ToString();
                invoice.InvoiceD = InvoceDate.ToString();

                
                return true;


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
            bool match = false;
            
            List<string> TotaltVärde = new List<string>();
            List<double> TotalMoms = new List<double>();
            double MomsTjugoFem = 1.25;
            double MomsTolv = 1.12;
            double MomsSex = 1.06;
            var lenght = MoneyList.Count;
            string patternDecimal = @"^\d+[\.](\d{1,2})$";
            Regex rgx = new Regex(patternDecimal);

            for (int i = 0; i < lenght; i++)
            {
                bool containsNum1 = Regex.IsMatch(MoneyList[i], @"^\d+[\.](\d{1,2})$");
                if (containsNum1)
                {
                    var value = MoneyList[i];
                        MoneyList.Remove(value);
                        var newValue = value.Replace(".", ",");
                        MoneyList.Add(newValue);

                    i--;
                }

            }
           
            
                double max = Convert.ToDouble(MoneyList.OrderByDescending(v => double.Parse(v)).FirstOrDefault());
                var max2 = MoneyList.OrderByDescending(v => double.Parse( v)).FirstOrDefault();
                //Öres utjämning
                //var deci = max - Math.Truncate(max);
                //if (deci >= 0.5)
                //{
                //    max++;
                //}
                //foreach(var it in )
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
                    if (Math.Truncate(resMoms25) == Math.Truncate(Convert.ToDouble(item)))
                    {
                        match = true;
                        TotaltVärde.Add(max.ToString());
                        TotalMoms.Add(Math.Truncate(resMoms25));
                        eInvoice.Amount = max;
                        eInvoice.VatAmount = Math.Truncate(resMoms25);
                    }
                    else if (Math.Truncate(resMoms12) == Math.Truncate(Convert.ToDouble(item)))
                    {
                        match = true;
                        TotaltVärde.Add(max.ToString());
                        TotalMoms.Add(resMoms25);
                        eInvoice.Amount = max;
                        eInvoice.VatAmount = Math.Truncate(resMoms12);
                    }
                    else if (Math.Truncate(resMoms6) == Math.Truncate(Convert.ToDouble(item)))
                    {
                        match = true;
                        TotaltVärde.Add(max.ToString());
                        TotalMoms.Add(resMoms25);
                        eInvoice.Amount = max;
                        eInvoice.VatAmount = Math.Truncate(resMoms6);
                    }

                }
                //om talet inte matchar
                if(match==false)
                {

                    if (MoneyList.Count == 1)
                    {
                        var max3 = Program.tempCashList.OrderByDescending(v => double.Parse(v)).FirstOrDefault();
                        eInvoice.Amount = Convert.ToDouble(max3);

                    }
                    else
                    {
                        MoneyList.Remove(max2);
                        getTheMaxValueAndCalculateMons(MoneyList, eInvoice);
                    }
                }

        }
        public static void createANewModel()
        {
            
        }
        public static void kollaAlltData(eInvoice eInvoice)
        {
            //Om viktig data är null skicka filen till oläsbar och välj nästa
            if(eInvoice.images1.Count == 0 || eInvoice.Amount == 0  || eInvoice.DueD == null ||eInvoice.InvoiceD ==null || (eInvoice.OrgNo == null && eInvoice.Pg == null && eInvoice.Bg == null))
            {
                eInvoice.Redable = false;
               //BildLäs.FindAndKillProcess("AcroRd32.exe");
               // string path = @"C:\OläsbaraPdf\";
               // Program.MovePdf(Program.FilePhth, path+Program.FileName);
               // var pdf = Program.ReadFiles();
               // int NrOfPages = Program.NumberOfPagesPdf(pdf);
               // await Program.ExtraktAndGetImages(pdf, NrOfPages);
            }
            else
            {
                eInvoice.Redable = true;
            }
        }
        public static async Task PostModelToController(eInvoice ListOfData)
        {
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

       

        public static async Task saveToDb(eInvoice invoice)
        {
            var context = new WebApplication1Context();
            context.eInvoice.Add(invoice);
            await context.SaveChangesAsync();
            var pdf = Program.ReadFiles();
            int NrOfPages = Program.NumberOfPagesPdf(pdf);
            await Program.ExtraktAndGetImages(pdf, NrOfPages);
        }

        public static async Task PosErrorPdf(eInvoice ListOfData)
        {
            var jsonString = await Task.Run(() => JsonConvert.SerializeObject(ListOfData, Formatting.None, new JavaScriptDateTimeConverter()));
            //string javascriptJson = JsonConvert.SerializeObject(jsonString);
            var str = new StringContent(jsonString, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(30);
                httpClient.BaseAddress = new Uri("http://localhost:26695/");
                await httpClient.PostAsync("/eInvoices/TestPost", str);
            }
        }

        public static List<string> getTheTotalAmount(List<String> WordList)   
        {
            
            bool wordListContainsNuberWhitComa = false;
            bool wordListContainsNuberWhitDot = false;
            
            var lenght = WordList.Count();
            List<string> PengarLista = new List<string>();
            
            
            string patternComa = @"(^\d{1,7})+(,\d{1,2}$)";
            
           
            Regex coma = new Regex(patternComa);
            foreach(var item in WordList)
            {
                if (Regex.IsMatch(item, @"(^\d{1,7})+(,\d{1,2}$)"))
                {
                    wordListContainsNuberWhitComa = true;
                }
                else if (Regex.IsMatch(item, @"(^\d{1,7})+([\.]\d{1,2}$)"))
                {
                    wordListContainsNuberWhitDot = true;
                }
               
            }
            if (wordListContainsNuberWhitComa == true)
            {
                string matchPattern1 = @"(^\d{3})+(,\d{1,2}$)";
                string matchPattern10 = @"(^\d{1,7})+(,\d{1,2}$)";
              PengarLista =  chekingIfValuesIsaMatch(WordList, matchPattern1 , matchPattern10);
            }
            else if(wordListContainsNuberWhitDot== true)
            {
                string matchPattern2 = @"(^\d{3})+([\.]\d{1,2}$)";
                string matchPattern20 = @"(^\d{1,7})+([\.]\d{1,2}$)";
                PengarLista = chekingIfValuesIsaMatch(WordList , matchPattern2 , matchPattern20);
            }
            else
            {
                string matchPattern3 = @"(^\d{3}$)";
                string matchPattern30 = @"(^\d{1,7}$)";
                PengarLista = chekingIfValuesIsaMatch(WordList, matchPattern3 ,matchPattern30);
            }
           
            

            return PengarLista;
        }
        public static List<string> chekingIfValuesIsaMatch(List<string> WordList, string matchPattern3d , string matchPattern7d)
        {
            string patternRenHundra = @"^(^\d{1,3})$";
            Regex rgx2 = new Regex(patternRenHundra);
            Regex rgx = new Regex(matchPattern7d);
            int i;
            var lenght = WordList.Count();
            List<string> PengarLista = new List<string>();
            for (i = 0; i < lenght; i++)
            {

                //kollar om talet har från 1 till 7 siffror och är comma separerat med 2 decimaler
                foreach (Match match in rgx.Matches(WordList[i]))
                {
                    if (match.Success == true)
                    {


                        bool containsNum = Regex.IsMatch(match.Value, matchPattern3d);
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
                                            bool containsNum3 = Regex.IsMatch(WordList[i - 2], @"^(^\d{3})$");
                                            if (containsNum3)
                                            {
                                                //om nummret inehåller 3 siffror kolla om föregående tal är en match
                                                foreach (Match match3 in rgx2.Matches(WordList[i - 2]))
                                                {
                                                    if (match3.Success == true)
                                                    {
                                                        var sum1 = match3.Value + match2.Value + match.Value;
                                                        bool containsNum4 = Regex.IsMatch(match3.Value, @"^(^\d{3})$");
                                                        if (containsNum4)
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
        public static void orgNrMatchning(List<string> WordList, eInvoice invoice) {
            int lenght = WordList.Count();
            string patternOrgNr = @"^[0-9]{10}$";
            string patternOrgNrWhitALineInIt = @"^([0-9]{6}-[0-9]{4})$";
            string patternVatNr = @"^([SE]+[0-9]{12})$";
            Regex rgx = new Regex(patternOrgNr);
            Regex rgx2 = new Regex(patternOrgNrWhitALineInIt);
            Regex rgx3 = new Regex(patternVatNr);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                if (WordList[i] == "Organisationsnummer" || WordList[i] == "company" && WordList[i + 1] == "reg" && WordList[i + 2] == "no" || WordList[i] == "vat" && WordList[i + 1] == "no" || WordList[i] == "momsnr"  || WordList[i] == "our" && WordList[i +1] == "vat-no" || WordList[i] == "vat" || WordList[i] == "vat" && WordList[i + 1] == "number" || WordList[i] == "vat" && WordList[i + 1] == "reg" && WordList[i + 2] == "no" || WordList[i] == "vat.nr:" || WordList[i] == "vårt" && WordList[i + 1] == "moms" && WordList[i + 2] == "reg.nr" || WordList[i] == "VATno" || WordList[i] == "Corporate" && WordList[i + 1] == "ID" || WordList[i] == "REG" && WordList[i + 1] == "NO:" )
                { 
                    for(int j = i; j <lenght; j++)
                    {
                        
                        foreach (Match match in rgx3.Matches(WordList[j]))
                        {
                            if (match.Success == true &&  WordList[i] != "SE556695422701")
                            {
                                string selectedWord = WordList[j];
                                for(int k = 0; k < WordList[j].Length; k++)
                                {
                                    if (k >= 2 && k < 12)
                                    {
                                        stringBuilder.Append(selectedWord[k]);
                                    }
                                }
                                if (true == checkIfItIsAOrgNr(stringBuilder.ToString()))
                                {
                                    
                                    invoice.OrgNo = stringBuilder.ToString();
                                    break;
                                }
                            }
                        }
                        foreach (Match match in rgx2.Matches(WordList[j]))
                        {
                            if (match.Success == true && WordList[j] !="556695-4227")
                            {

                                if (true == checkIfItIsAOrgNr(WordList[j]))
                                {

                                    invoice.OrgNo = WordList[j];
                                    break;
                                }
                            }
                        }
                        foreach (Match match in rgx.Matches(WordList[j]))
                        {
                            if (match.Success == true &&  WordList[j] != "5566954227")
                            {
                                if (true == checkIfItIsAOrgNr(WordList[j]))
                                {
                                   
                                    invoice.OrgNo = WordList[j];
                                    break;
                                }
                            }
                        }
                        
                    }

                }
        }
        }
            public static void GetOrgNumber(List<string> WordList , eInvoice invoice)
        {
            int i;
            var lenght = WordList.Count();
            List<string> OrgNrList = new List<string>();
            string patternOrgNr = @"^[0-9]{10}$";
            string patternOrgNrWhitALineInIt = @"^[0-9]{6}-[0-9]{4}$";
            Regex rgx = new Regex(patternOrgNr);
            Regex rgx2 = new Regex(patternOrgNrWhitALineInIt);

            for (i = 0; i < lenght; i++)
            {
                
                foreach (Match match in rgx.Matches(WordList[i]) )
                {
                    if (match.Success == true && WordList[i] != "556695-4227" || WordList[i] != "5566954227")
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
                    if (match.Success == true && WordList[i] != "556695-4227")
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
            var stringtSum = tSum.ToString();
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
        static public bool FindAndKillProcess(string name)
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes by using the StartsWith Method,
                //this prevents us from incluing the .EXE for the process we're looking for.
                //. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running
                if (clsProcess.ProcessName.StartsWith(name))
                {
                    //since we found the proccess we now need to use the
                    //Kill Method to kill the process. Remember, if you have
                    //the process running more than once, say IE open 4
                    //times the loop thr way it is now will close all 4,
                    //if you want it to just close the first one it finds
                    //then add a return; after the Kill
                    clsProcess.Kill();
                    //process killed, return true
                    return true;
                }
            }
            //process not found, return false
            return false;
        }

    }
 }
    



