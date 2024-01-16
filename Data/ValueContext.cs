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
        public ValueContext (DbContextOptions<ValueContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>()
                .HasNoKey();                
        }



        public DbSet<TestTask.Models.Value> DataModel { get; set; } = default!;
    }
}
