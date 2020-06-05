using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travels.Data;
using Travels.Models;
using Travels.Models.ViewModel;
using Travels.Utility;

namespace Travels.Pages.Places
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Travels.Data.ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EditModel(Travels.Data.ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public Place Place { get; set; }
        [BindProperty]
        public PlacesAndUserViewModel PlacesAndUserVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Place = await _context.Place
                .Include(p => p.ApplicationUser).Include(p => p.TravelType).FirstOrDefaultAsync(m => m.Id == id);

            PlacesAndUserVM = new PlacesAndUserViewModel();


            IQueryable<TravelType> lstTravelType = from s in _context.TravelType select s;

            PlacesAndUserVM.TravelTypesList = lstTravelType.ToList();

            if (Place == null)
            {
                return NotFound();
            }
           ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string img = ImgFromDb(Place);

            _context.Attach(Place).State = EntityState.Modified;

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var PlaceFromdb = _context.Place.Find(Place.Id);

            if (files.Count == 0)
            {
                Place.Image = img;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, Place.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, Place.Id + extension));
                }

                extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));
                using (var fileStream = new FileStream(Path.Combine(uploads, Place.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }
                Place.Image = @"\images\" + Place.Id + extension;
            }
            PlaceFromdb.Active = "YES";
            await _context.SaveChangesAsync();
            return RedirectToPage("Index", new { userId = PlaceFromdb.UserId });
        }

        public string ImgFromDb(Place Place)
        {
            var PlaceFromdb = _context.Place.Find(Place.Id);
            string img = PlaceFromdb.Image;
            var local = _context.Set<Place>()
            .Local
            .FirstOrDefault(entry => entry.Id.Equals(Place.Id));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.SaveChanges();
            return img;
        }
    }
}
