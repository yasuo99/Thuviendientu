﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThuVienDienTu.Data;
using ThuVienDienTu.Models;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
   // [Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
    [Area("Admin")]
    public class GenresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Genres
        public async Task<IActionResult> Index()
        {
            return View(await _context.Genres.ToListAsync());
        }

        // GET: Admin/Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }

        // GET: Admin/Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GenresName,Description")] Genres genres)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genres);
        }

        // GET: Admin/Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres.FindAsync(id);
            if (genres == null)
            {
                return NotFound();
            }
            return View(genres);
        }

        // POST: Admin/Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GenresName,Description")] Genres genres)
        {
            if (id != genres.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genres);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenresExists(genres.Id))
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
            return View(genres);
        }

        // GET: Admin/Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genres = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genres == null)
            {
                return NotFound();
            }

            return View(genres);
        }

        // POST: Admin/Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genres = await _context.Genres.FindAsync(id);
            _context.Genres.Remove(genres);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenresExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}
