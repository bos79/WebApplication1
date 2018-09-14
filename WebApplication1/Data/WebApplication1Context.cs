using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class WebApplication1Context : DbContext
    {
        public WebApplication1Context()
        {
        }

        public WebApplication1Context (DbContextOptions<WebApplication1Context> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.eInvoice> eInvoice { get; set; }

        public DbSet<WebApplication1.Models.Images> Images { get; set; }

        public DbSet<WebApplication1.Models.customerModel> customerModel { get; set; }

        public DbSet<WebApplication1.Models.companyModell> companyModell { get; set; }

        public DbSet<WebApplication1.Models.suplierModel> suplierModel { get; set; }

        //public DbSet<WebApplication1.Models.File> File { get; set; }
    }
}
