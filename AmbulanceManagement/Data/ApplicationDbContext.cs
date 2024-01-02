using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AmbulanceManagement.Models;

namespace AmbulanceManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AmbulanceManagement.Models.Patient> Patient { get; set; } = default!;
    }
}
