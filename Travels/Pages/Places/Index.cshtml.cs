using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Travels.Data;
using Travels.Models;
using Travels.Models.ViewModel;
using Travels.Utility;

namespace Travels.Pages.Places
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public PlacesAndUserViewModel PlacesAndUserVM { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public Place Place { get; set; }

        public async Task<IActionResult> OnGet(int productPage = 1, string userId = null, string searchCountry = null, string searchLocality = null, string searchType = null)
        {
            if(userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }

            PlacesAndUserVM = new PlacesAndUserViewModel()
            {
                Place = await _db.Place.Where(t => t.UserId == userId).ToListAsync(),
                UserObj = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == userId),
                TravelTypesList = await _db.TravelType.ToListAsync(),
                PlacesList = await _db.Place.Where(t => t.Active == "YES").ToListAsync()
            };

            StringBuilder parm = new StringBuilder();
            parm.Append("/Places?productPage=:");
            parm.Append("&userId=");
            if (userId != null)
            {
                parm.Append(userId);
            }
            parm.Append("&searchCountry");
            if (searchCountry != null)
            {
                parm.Append(searchCountry);
            }
            parm.Append("&searchLocality");
            if (searchCountry != null)
            {
                parm.Append(searchLocality);
            }
            parm.Append("&searchType");
            if (searchCountry != null)
            {
                parm.Append(searchType);
            }

            if (searchCountry != null)
            {
                PlacesAndUserVM.PlacesList = await _db.Place.Where(u => u.Country.ToLower().Contains(searchCountry.ToLower())).ToListAsync();
            }
            else
            {
                if (searchLocality != null)
                {
                    PlacesAndUserVM.PlacesList = await _db.Place.Where(u => u.Locality.ToLower().Contains(searchLocality.ToLower())).ToListAsync();
                }
                else
                {
                    if (searchType != null)
                    {
                        PlacesAndUserVM.PlacesList = await _db.Place.Where(u => u.TravelType.Name.ToLower().Contains(searchType.ToLower())).ToListAsync();
                    }
                }
            }

            var count = PlacesAndUserVM.PlacesList.Count;
            PlacesAndUserVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParm = parm.ToString()
            };

            PlacesAndUserVM.PlacesList = PlacesAndUserVM.PlacesList.OrderBy(p => p.Country)
                .Skip((productPage - 1) * SD.PaginationUsersPageSize)
                .Take(SD.PaginationUsersPageSize).ToList();

            return Page();
        }
    }
}