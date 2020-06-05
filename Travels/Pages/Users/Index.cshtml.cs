using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Travels.Data;
using Travels.Models;
using Travels.Models.ViewModel;
using Travels.Utility;

namespace Travels.Pages.Users
{
    [Authorize(Roles = SD.AdminEndUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public UsersListViewModel UsersListVM { get; set; }

        public async Task<IActionResult> OnGet(int productPage=1, string searchEmail=null, string searchFirstName= null, string searchLastName = null)
        {
            UsersListVM = new UsersListViewModel()
            {
                ApplicationUserList = await _db.ApplicationUser.ToListAsync()
            };

            StringBuilder parm = new StringBuilder();
            parm.Append("/Users?productPage=:");
            parm.Append("&searchEmail");
            if(searchEmail != null)
            {
                parm.Append(searchEmail);
            }
            parm.Append("&searchFirstName");
            if (searchEmail != null)
            {
                parm.Append(searchFirstName);
            }
            parm.Append("&searchLastName");
            if (searchEmail != null)
            {
                parm.Append(searchLastName);
            }

            if(searchEmail!=null)
            {
                UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).ToListAsync();
            }
            else
            {
                if (searchFirstName != null)
                {
                    UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.FirstName.ToLower().Contains(searchFirstName.ToLower())).ToListAsync();
                }
                else
                {
                    if (searchLastName != null)
                    {
                        UsersListVM.ApplicationUserList = await _db.ApplicationUser.Where(u => u.LastName.ToLower().Contains(searchLastName.ToLower())).ToListAsync();
                    }
                }
            }

            var count = UsersListVM.ApplicationUserList.Count;
            UsersListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = SD.PaginationUsersPageSize,
                TotalItems = count,
                UrlParm = parm.ToString()
            };

            UsersListVM.ApplicationUserList = UsersListVM.ApplicationUserList.OrderBy(p => p.Email)
                .Skip((productPage - 1) * SD.PaginationUsersPageSize)
                .Take(SD.PaginationUsersPageSize).ToList();

            return Page();
        }
    }
}