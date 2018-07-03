using System;
using System.Collections.Generic;
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

        public eInvoicesController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: eInvoices
        public async Task<IActionResult> Index()
        {
            //using (StreamReader sr = new StreamReader(Server.MapPath("~/Content/treatments.json")))
            //{
            //    treatments = JsonConvert.DeserializeObject<List<Treatment>>(sr.ReadToEnd());
            //}
            return View(await _context.eInvoice.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("eInvoiceId,CurrencyCode,PurchOrderNo,PayType,Amount,InvoiceDate,InvoiceDateSpecified,DueDate,DueDateSpecified,VatAmount,Authorizor,OurCustomerNo,InvoiceType,Supplier,SupplierNo,InvoiceNo,Ocr,Freight,FreightSpecified,VatCode,Pg,Bg,OrgNo,OurReference,YourReference,InvoiceRecipient,Address,PostCode,City,Message,Vat,InvoiceFee,Project")] eInvoice eInvoice)
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
