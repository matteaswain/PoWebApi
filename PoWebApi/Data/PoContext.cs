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

        public DbSet<Employee> Employee { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<PoLine> PoLines { get; set; }

// This sets our Login as unique
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Employee>(e => { e.HasIndex(p => p.Login).IsUnique(); });
        }
    }
}
