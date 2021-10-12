using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Data;
using Wba.StovePalace.Models;

using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Wba.StovePalace.Pages.Stoves
{
    public class EditModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;
        private readonly IWebHostEnvironment webhostEnvironment;
        public EditModel(Wba.StovePalace.Data.StoveContext context,
            IWebHostEnvironment webhostEnvironment)
        {
            _context = context;
            this.webhostEnvironment = webhostEnvironment;

        }


        [BindProperty]
        public Stove Stove { get; set; }
        [BindProperty]
        public IFormFile PhotoUpload { get; set; }

        //public IActionResult OnGet(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    Stove =  _context.Stove
        //        .Include(s => s.Brand)
        //        .Include(s => s.Fuel).FirstOrDefault(m => m.Id == id);
        //    if (Stove == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(b => b.BrandName).ToList(), "Id", "BrandName");
        //    ViewData["FuelId"] = new SelectList(_context.Fuel.OrderBy(f => f.FuelName).ToList(), "Id", "FuelName");
        //    return Page();
        //}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Stove = await _context.Stove
                .Include(s => s.Brand)
                .Include(s => s.Fuel).FirstOrDefaultAsync(m => m.Id == id);

            if (Stove == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(b => b.BrandName).ToList(), "Id", "BrandName");
            ViewData["FuelId"] = new SelectList(_context.Fuel.OrderBy(f => f.FuelName).ToList(), "Id", "FuelName");
            return Page();
        }

        
        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            if (PhotoUpload != null)
            {
                if (Stove.ImagePath != null)
                {
                    string filePath = Path.Combine(webhostEnvironment.WebRootPath, "images", Stove.ImagePath);
                    System.IO.File.Delete(filePath);
                }
                Stove.ImagePath = ProcessUploadedFile();
            }


            _context.Add(Stove).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoveExists(Stove.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StoveExists(int id)
        {
            return _context.Stove.Any(e => e.Id == id);
        }

        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;
            if (PhotoUpload != null)
            {
                string uploadFolder = Path.Combine(webhostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + PhotoUpload.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    PhotoUpload.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
