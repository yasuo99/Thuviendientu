using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using ThuVienDienTu.Data;
using ThuVienDienTu.DesignPatterns.RepositoryPatterns;
using ThuVienDienTu.Models;
using ThuVienDienTu.Models.ViewModels;
using ThuVienDienTu.Utility;

namespace ThuVienDienTu.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ADMIN_ROLE + "," + SD.LIBRARIAN_ROLE)]
    [Area("Admin")]
    public class CountriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IUnitOfWork UnitOfWork;
        [BindProperty]
        public CountryViewModel CountriesVM { get; set; }

        public CountriesController(ApplicationDbContext context)
        {
            _context = context;
            this.UnitOfWork = new UnitOfWork(_context);
            CountriesVM = new CountryViewModel()
            {
                Country = new Country(),
                Total = 0
            };
        }
        // GET: Admin/Countries
        public async Task<IActionResult> Index()
        {
            var countries = await UnitOfWork.Countries.Get();
            CountriesVM.Countries = countries.ToList();
            CountriesVM.Total = await UnitOfWork.Countries.CountAll();
            return View(CountriesVM);
        }

        // GET: Admin/Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var country = await UnitOfWork.Countries.GetById(id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Admin/Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryName")] Country country)
        {
            if (ModelState.IsValid)
            {
                await UnitOfWork.Countries.Insert(country);
                UnitOfWork.Commit();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Admin/Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await UnitOfWork.Countries.GetById(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Admin/Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryName")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await UnitOfWork.Countries.Update(country);
                    UnitOfWork.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
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
            return View(country);
        }

        // GET: Admin/Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await UnitOfWork.Countries.GetById(id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Admin/Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var country = await UnitOfWork.Countries.GetById(id);
            await UnitOfWork.Countries.Delete(country);
            UnitOfWork.Commit();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
