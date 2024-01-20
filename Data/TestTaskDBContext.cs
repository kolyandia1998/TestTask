
using Microsoft.EntityFrameworkCore;
using TestTask.Models;

namespace TestTask.Data
{
    public class TestTaskDBContext : DbContext
    {
        public  TestTaskDBContext (DbContextOptions<TestTaskDBContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Value>().HasKey(v => new { v.FileName, v.Date });
            modelBuilder.Entity<Result>().HasKey(v =>  v.FileName );
        }
        public DbSet<TestTask.Models.Value> Values { get; set; } = default!;
        public DbSet<TestTask.Models.Result> Results { get; set; } = default!;
    }
}
