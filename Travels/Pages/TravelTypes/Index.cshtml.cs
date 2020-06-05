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
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public IList<TravelType> TravelType { get; set; }

        public async Task<IActionResult> OnGet()
        {
            TravelType = await _db.TravelType.ToListAsync();
            return Page();
        }
    }
}