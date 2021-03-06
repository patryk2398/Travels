﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Travels.Data;
using Travels.Models;

namespace Travels.Pages.Places
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly Travels.Data.ApplicationDbContext _context;

        public DetailsModel(Travels.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Place Place { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await _context.Place
                .Include(p => p.ApplicationUser).Include(p => p.TravelType).FirstOrDefaultAsync(m => m.Id == id);

            if (Place == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
