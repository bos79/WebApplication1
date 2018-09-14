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
    public class customerModelsController : Controller
    {
        private readonly WebApplication1Context _context;

        public customerModelsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: customerModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.customerModel.ToListAsync());
        }

        // GET: customerModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerModel = await _context.customerModel
                .SingleOrDefaultAsync(m => m.customerId == id);
            if (customerModel == null)
            {
                return NotFound();
            }

            return View(customerModel);
        }

        // GET: customerModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: customerModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("customerId,Name,eMail")] customerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerModel);
        }

        // GET: customerModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerModel = await _context.customerModel.SingleOrDefaultAsync(m => m.customerId == id);
            if (customerModel == null)
            {
                return NotFound();
            }
            return View(customerModel);
        }

        // POST: customerModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("customerId,Name,eMail")] customerModel customerModel)
        {
            if (id != customerModel.customerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!customerModelExists(customerModel.customerId))
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
            return View(customerModel);
        }

        // GET: customerModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerModel = await _context.customerModel
                .SingleOrDefaultAsync(m => m.customerId == id);
            if (customerModel == null)
            {
                return NotFound();
            }

            return View(customerModel);
        }

        // POST: customerModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerModel = await _context.customerModel.SingleOrDefaultAsync(m => m.customerId == id);
            _context.customerModel.Remove(customerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool customerModelExists(int id)
        {
            return _context.customerModel.Any(e => e.customerId == id);
        }
    }
}
