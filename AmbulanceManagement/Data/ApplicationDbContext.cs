using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AmbulanceManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AmbulanceManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AmbulanceManagement.Models.Patient> Patient { get; set; } = default!;
        public DbSet<AmbulanceManagement.Models.Visit> Visit { get; set; } = default!;
    }
}
