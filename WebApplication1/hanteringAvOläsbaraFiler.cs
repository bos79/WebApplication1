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
    public class hanteringAvOläsbaraFiler
    {
        public static List<string> PdfList;
        public static string ReadFiles()
        {
            /*string path = @"C:\AvlästaPdf\"*/;
            //if (Program.FilePhth != null)
            //{
            //    MovePdf(Program.FilePhth, path + FileName);
            //}
            //if (Program.count > 1)
            //{
            //    Program.count = 1;
            //}
            //if (Program.CombineList != null)
            //{
            //    Program.CombineList.Clear();
            //}
            string PdfPath = @"C:\OläsbaraPdf";
            PdfList = Directory.GetFiles(PdfPath, "*.pdf").ToList();
            if (PdfList.Count == 0)
            {
                return null;
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
    }
}