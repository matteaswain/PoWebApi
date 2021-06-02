using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoWebApi.Models;

namespace PoWebApi.Data
{
    public class PoContext : DbContext
    {
        public PoContext (DbContextOptions<PoContext> options)
            : base(options)
        {
        }

        public DbSet<PoWebApi.Models.Employee> Employee { get; set; }
    }
}
