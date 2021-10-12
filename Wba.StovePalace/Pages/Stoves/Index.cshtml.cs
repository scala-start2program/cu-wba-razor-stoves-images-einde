using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wba.StovePalace.Models;

namespace Wba.StovePalace.Pages.Stoves
{
    public class IndexModel : PageModel
    {
        private readonly Wba.StovePalace.Data.StoveContext _context;

        public IndexModel(Wba.StovePalace.Data.StoveContext context)
        {
            _context = context;
        }

        public IList<Stove> Stoves { get;set; }
        public List<SelectListItem> AvailableBrands { get; set; }
        public List<SelectListItem> AvailableFuels { get; set; }
        public int? SelectedBrandId { get; set; }
        public int? SelectedFuelId { get; set; }


        public void OnGet()
        {
            PopulateCollections();
        }
        private void PopulateCollections()
        {
            Stoves = _context.Stove
                .Include(b => b.Brand)
                .Include(f => f.Fuel).ToList();
            Stoves = Stoves.OrderBy(s => s.Brand.BrandName)
                .ThenBy(s => s.Fuel.FuelName).ToList();
            AvailableBrands = _context.Brand.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.BrandName
                }).ToList();
            AvailableBrands = AvailableBrands.OrderBy(b => b.Text).ToList();
            AvailableFuels = _context.Fuel.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FuelName
                }).ToList();
            AvailableFuels = AvailableFuels.OrderBy(b => b.Text).ToList();

            AvailableBrands.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle merken ---"
            });
            AvailableFuels.Insert(0, new SelectListItem()
            {
                Value = null,
                Text = "--- Alle brandstoffen ---"
            });
        }
        public void OnPost(int ? brandFilter, int? fuelFilter)
        {
            SelectedBrandId = brandFilter;
            SelectedFuelId = fuelFilter;
            PopulateCollections();

            if (brandFilter == null && fuelFilter == null)
            {
                return;
            }
            else if (brandFilter != null && fuelFilter == null)
            {
                Stoves = (from x in Stoves where (x.BrandId == brandFilter) select x).ToList();
            }
            else if (brandFilter == null && fuelFilter != null)
            {
                Stoves = (from x in Stoves where (x.FuelId == fuelFilter) select x).ToList();
            }
            else
            {
                Stoves = (from x in Stoves where (x.BrandId == brandFilter && x.FuelId == fuelFilter) select x).ToList();
            }
        }


    }
}
