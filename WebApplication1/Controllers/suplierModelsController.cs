using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class suplierModelsController : Controller
    {
        private readonly WebApplication1Context _context;

        public suplierModelsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: suplierModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.suplierModel.ToListAsync());
        }

        // GET: suplierModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suplierModel = await _context.suplierModel
                .SingleOrDefaultAsync(m => m.suplierId == id);
            if (suplierModel == null)
            {
                return NotFound();
            }

            return View(suplierModel);
        }
        // GET: suplierModels/Details/5
        [HttpPost]
        public  async Task<suplierModel> getSuplierByOrgNr([FromBody] eInvoice eInvoice)
        {
            if (eInvoice.OrgNo == null)
            {
                return null;
            }

            var suplierModel = await _context.suplierModel
                .SingleOrDefaultAsync(m => m.suplierOrgNr == eInvoice.OrgNo);
            if (suplierModel == null)
            {
                return null;
            }

            return suplierModel;
        }

        // GET: suplierModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: suplierModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("suplierId,suplierName,suplierOrgNr,suplierPgNr,suplierBgNr,currencyCode,suplierOrgNrFormat,suplierOrgNrWordMatch,suplierPgNrFormat,suplierPgNrWordMatch,suplierBgNrFormat,suplierBgNrWordMatch,currencyFormat,currencyWordMatch,purchOrderNoFormat,purchOrderNoWordMatch,invoiceDateFormat,invoiceDateWordMatch,suplierOrcNrFormat,suplierOrcNrWordMatch")] suplierModel suplierModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suplierModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suplierModel);
        }

        // GET: suplierModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suplierModel = await _context.suplierModel.SingleOrDefaultAsync(m => m.suplierId == id);
            if (suplierModel == null)
            {
                return NotFound();
            }
            return View(suplierModel);
        }

        // POST: suplierModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("suplierId,suplierName,suplierOrgNr,suplierPgNr,suplierBgNr,currencyCode,suplierOrgNrFormat,suplierOrgNrWordMatch,suplierPgNrFormat,suplierPgNrWordMatch,suplierBgNrFormat,suplierBgNrWordMatch,currencyFormat,currencyWordMatch,purchOrderNoFormat,purchOrderNoWordMatch,invoiceDateFormat,invoiceDateWordMatch,suplierOrcNrFormat,suplierOrcNrWordMatch")] suplierModel suplierModel)
        {
            if (id != suplierModel.suplierId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suplierModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!suplierModelExists(suplierModel.suplierId))
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
            return View(suplierModel);
        }

        // GET: suplierModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suplierModel = await _context.suplierModel
                .SingleOrDefaultAsync(m => m.suplierId == id);
            if (suplierModel == null)
            {
                return NotFound();
            }

            return View(suplierModel);
        }

        // POST: suplierModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suplierModel = await _context.suplierModel.SingleOrDefaultAsync(m => m.suplierId == id);
            _context.suplierModel.Remove(suplierModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool suplierModelExists(int id)
        {
            return _context.suplierModel.Any(e => e.suplierId == id);
        }
    }
}
