using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Travels.Data;
using Travels.Models;
using Travels.Utility;

namespace Travels.Pages.TravelTypes
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public TravelType TravelType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TravelType = await _db.TravelType.FirstOrDefaultAsync(m => m.Id == id);

            if (TravelType == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TravelType == null)
            {
                return NotFound();
            }

            _db.TravelType.Remove(TravelType);
            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}