using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travels.Models;

namespace Travels.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TravelType> TravelType { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Place> Place { get; set; }
    }
}
