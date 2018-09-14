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
    public class companyModellsController : Controller
    {
        private readonly WebApplication1Context _context;

        public companyModellsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: companyModells
        public async Task<IActionResult> Index()
        {
            return View(await _context.companyModell.ToListAsync());
        }

        // GET: companyModells/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyModell = await _context.companyModell
                .SingleOrDefaultAsync(m => m.companyId == id);
            if (companyModell == null)
            {
                return NotFound();
            }

            return View(companyModell);
        }

        // GET: companyModells/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: companyModells/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("companyId,companyOrgNr,compayPgNr,companyName")] companyModell companyModell)
        {
            if (ModelState.IsValid)
            {
                _context.Add(companyModell);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(companyModell);
        }

        // GET: companyModells/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyModell = await _context.companyModell.SingleOrDefaultAsync(m => m.companyId == id);
            if (companyModell == null)
            {
                return NotFound();
            }
            return View(companyModell);
        }

        // POST: companyModells/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("companyId,companyOrgNr,compayPgNr,companyName")] companyModell companyModell)
        {
            if (id != companyModell.companyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyModell);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!companyModellExists(companyModell.companyId))
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
            return View(companyModell);
        }

        // GET: companyModells/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyModell = await _context.companyModell
                .SingleOrDefaultAsync(m => m.companyId == id);
            if (companyModell == null)
            {
                return NotFound();
            }

            return View(companyModell);
        }

        // POST: companyModells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var companyModell = await _context.companyModell.SingleOrDefaultAsync(m => m.companyId == id);
            _context.companyModell.Remove(companyModell);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool companyModellExists(int id)
        {
            return _context.companyModell.Any(e => e.companyId == id);
        }
    }
}
