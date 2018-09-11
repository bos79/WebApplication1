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
    public class DBRepositoryContext : DbContext
    {
        public DBRepositoryContext() : base()
        {
        }

        public DBRepositoryContext(DbContextOptions<DBRepositoryContext> options)
            : base(options)
        {
        }
        public DbSet<WebApplication1.Models.eInvoice> eInvoice { get; set; }

        public DbSet<WebApplication1.Models.Images> Images { get; set; }


        

    }
}
