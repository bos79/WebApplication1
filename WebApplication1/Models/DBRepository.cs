using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.Models
{
    public class DBRepository
    {
        private readonly DBRepositoryContext db;

        public  DBRepository(DBRepositoryContext context)
        {
            db = context;

        }
        public List<Images> GetImages()
        {
            return db.Images.ToList();
        }
        public List<Images> GetImagesById(int id)
        {
            return db.Images.Where(m => m.ImagesId == id).ToList();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Save(eInvoice eInvoice)
        {

            db.eInvoice.Add(eInvoice);
            await db.SaveChangesAsync();

        }
    }
}
