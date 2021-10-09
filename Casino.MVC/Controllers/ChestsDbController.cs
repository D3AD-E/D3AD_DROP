using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CasinoMVC.Core;
using CasinoMVC.Data;
using CasinoMVC.Models;

namespace CasinoMVC.Controllers
{
    public class ChestsDbController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChestsDbController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ChestDb
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chests.ToListAsync());
        }

        // GET: ChestDb/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chestDbItem = await _context.Chests.FindAsync(id);
                
            if (chestDbItem == null)
            {
                return NotFound();
            }

            return View(chestDbItem);
        }

        // GET: ChestDb/Create
        public IActionResult Create()
        {
            AdminChestModel model = new AdminChestModel
            {
                AllItems = _context.DotaItems.ToList()
            };
            return View(model);
        }

        // POST: ChestDb/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminChestModel form)
        {
            var chest = form.Chest;
            if (ModelState.IsValid && chest is not null)
            {
                _context.Chests.Add(chest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Admin");
            }
            return RedirectToAction(nameof(Index), "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> AddChest(AdminChestModel form)
        {
            var chest = form.Chest;
            if (ModelState.IsValid && chest is not null)
            {
                _context.Chests.Add(chest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Admin");
            }
            return RedirectToAction(nameof(Index), "Admin");
        }

        // GET: ChestDb/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chestDbItem = await _context.Chests.FindAsync(id);
            if (chestDbItem == null)
            {
                return NotFound();
            }
            AdminChestModel model = new AdminChestModel
            {
                Chest = chestDbItem,
                AllItems = _context.DotaItems.ToList()
            };

            foreach (var itemId in chestDbItem.ItemIds.Keys)
            {
                var item = await _context.DotaItems.FindAsync(itemId);
                model.ChestItems.Add(item);
            }
            return View(model);
        }

        // POST: ChestDb/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminChestModel chestDbItem)
        {
            if (id != chestDbItem.Chest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chestDbItem.Chest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChestDbItemExists(chestDbItem.Chest.Id))
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
            return View(chestDbItem);
        }

        // GET: ChestDb/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chestDbItem = await _context.Chests.FindAsync(id);
            if (chestDbItem == null)
            {
                return NotFound();
            }

            return View(chestDbItem);
        }

        // POST: ChestDb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chestDbItem = await _context.Chests.FindAsync(id);
            _context.Chests.Remove(chestDbItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChestDbItemExists(int id)
        {
            return _context.Chests.Any(e => e.Id == id);
        }
    }
}
