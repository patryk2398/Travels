using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Travels.Data;
using Travels.Models;
using Travels.Utility;

namespace Travels.Pages.TravelTypes
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public TravelType TravelType { get; set; }

        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _db.TravelType.Add(TravelType);
            await _db.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}