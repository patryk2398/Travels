using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Travels.Data;
using Travels.Models;
using Travels.Models.ViewModel;
using Travels.Utility;

namespace Travels.Pages.Places
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public Place Place { get; set; }
        [BindProperty]
        public PlacesAndUserViewModel PlacesAndUserVM { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public CreateModel(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult OnGet(string userId = null)
        {
            Place = new Place();
            if (userId == null)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                userId = claim.Value;
            }
            Place.UserId = userId;

            PlacesAndUserVM = new PlacesAndUserViewModel();
            

            IQueryable<TravelType> lstTravelType = from s in _db.TravelType select s;

            PlacesAndUserVM.TravelTypesList = lstTravelType.ToList();

            return Page();
        }

        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _db.Place.Add(Place);
            await _db.SaveChangesAsync();
            StatusMessage = "Miejsce dodane";

            string webRootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = _db.Place.Find(Place.Id);
            menuItemFromDb.Active = "YES";

            if (files.Count == 0)
            {
                menuItemFromDb.Image = @"\images\default.jpg";
            }
            else
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                using (var fileStream = new FileStream(Path.Combine(uploads, Place.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                menuItemFromDb.Image = @"\images\" + Place.Id + extension;
            }

            await _db.SaveChangesAsync();

            return RedirectToPage("Index", new { userId = Place.UserId });
        }
    }
}