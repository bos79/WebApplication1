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

namespace WebApplication1.Controllers
{
    public class eInvoicesController : Controller
    {
        private readonly WebApplication1Context _context;
        //private DBRepository dBRepository = new DBRepository();

        public eInvoicesController(WebApplication1Context context)
        {
            _context = context;
            
        }
        [HttpPost]
        public async Task<IActionResult> TestPost([FromBody][Bind("eInvoiceId,CurrencyCode,PurchOrderNo,PayType,Amount,InvoiceDate,InvoiceDateSpecified,DueDate,DueDateSpecified,VatAmount,Authorizor,OurCustomerNo,InvoiceType,Supplier,SupplierNo,InvoiceNo,Ocr,Freight,FreightSpecified,VatCode,Pg,Bg,OrgNo,OurReference,YourReference,InvoiceRecipient,Address,PostCode,City,Message,Vat,InvoiceFee,Project,pdfPath,readable")]eInvoice invoice)
        {
            _context.Add(invoice);
            await _context.SaveChangesAsync();
            var pdf = Program.ReadFiles();
            int NrOfPages = Program.NumberOfPagesPdf(pdf);
            await Program.ExtraktAndGetImages(pdf, NrOfPages);
            return View();
          
        }
        // GET: eInvoices
        public async Task<IActionResult> Index()
        {
            //List< List<Images>> item = new List<List<Images>>();
            //item.Add( await _context.eInvoice.Select(m => m.images1).ToListAsync());

            //byte[] photoBack = item.InternalImage;
            eInvoice invoice = new eInvoice();
            invoice.ListImages = await _context.Images.ToListAsync();
            var invoiceList = await _context.eInvoice.ToListAsync();
            //invoiceList = _context.eInvoice.Include(m => m.ListImages);
            foreach(var item in invoiceList)
            {
                item.ListImages = await _context.Images.Where(m => m.eInvoiceId == item.eInvoiceId).ToListAsync();
            }
           

            
            return View(invoiceList);
        }
        public  async Task<string> RenderImage(int id )
        {
            

            var images = await _context.Images.FindAsync(id);
            byte[] photoBack = images.ImageData;
            Image rImage = null;
            //using (MemoryStream ms = new MemoryStream(photoBack))
            //{
            //    rImage = Image.FromStream(ms);
                //return File(photoBack, "image/jpg");
            //}
            string base64String = Convert.ToBase64String(photoBack, 0, photoBack.Length);
            var bild = "data:image/jpg;base64," + base64String;

            return bild;

        }
        [HttpPost]
        public JsonResult PostJsonData(eInvoice incomingData)
        {
            string status = null;
            try
            {
                //saveSquirrel(incomingData);
                status = "If you don't see this, something went wrong.";
            }
            catch (Exception e)
            {
                status = e.ToString();
            }
            return Json(status);
        }

        // GET: eInvoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eInvoice = await _context.eInvoice
                .SingleOrDefaultAsync(m => m.eInvoiceId == id);
            if (eInvoice == null)
            {
                return NotFound();
            }

            return View(eInvoice);
        }

        // GET: eInvoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: eInvoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("eInvoiceId,CurrencyCode,PurchOrderNo,PayType,Amount,InvoiceD,InvoiceDateSpecified,DueD,DueDateSpecified,VatAmount,Authorizor,OurCustomerNo,InvoiceType,Supplier,SupplierNo,InvoiceNo,Ocr,Freight,FreightSpecified,VatCode,Pg,Bg,OrgNo,OurReference,YourReference,InvoiceRecipient,Address,PostCode,City,Message,Vat,InvoiceFee,Project")] eInvoice eInvoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eInvoice);
        }

        // GET: eInvoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eInvoice = await _context.eInvoice.SingleOrDefaultAsync(m => m.eInvoiceId == id);
            if (eInvoice == null)
            {
                return NotFound();
            }
            return View(eInvoice);
        }

        // POST: eInvoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("eInvoiceId,CurrencyCode,PurchOrderNo,PayType,Amount,InvoiceDate,InvoiceDateSpecified,DueDate,DueDateSpecified,VatAmount,Authorizor,OurCustomerNo,InvoiceType,Supplier,SupplierNo,InvoiceNo,Ocr,Freight,FreightSpecified,VatCode,Pg,Bg,OrgNo,OurReference,YourReference,InvoiceRecipient,Address,PostCode,City,Message,Vat,InvoiceFee,Project")] eInvoice eInvoice)
        {
            if (id != eInvoice.eInvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eInvoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!eInvoiceExists(eInvoice.eInvoiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eInvoice);
        }

        // GET: eInvoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eInvoice = await _context.eInvoice
                .SingleOrDefaultAsync(m => m.eInvoiceId == id);
            if (eInvoice == null)
            {
                return NotFound();
            }

            return View(eInvoice);
        }

        // POST: eInvoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eInvoice = await _context.eInvoice.SingleOrDefaultAsync(m => m.eInvoiceId == id);
            _context.eInvoice.Remove(eInvoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool eInvoiceExists(int id)
        {
            return _context.eInvoice.Any(e => e.eInvoiceId == id);
        }
    }
}
