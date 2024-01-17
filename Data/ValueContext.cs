using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data
{
    public class ValueContext : DbContext
    {
        public  ValueContext (DbContextOptions<ValueContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>().HasKey(v => new { v.FileName, v.Date});
        }
        public DbSet<TestTask.Models.Value> Values { get; set; } = default!;
    }
}
