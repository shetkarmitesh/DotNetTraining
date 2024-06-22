using Libaray_Management_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Data
{
    public class IssueBookDBContext:DbContext
    {
        public IssueBookDBContext(DbContextOptions<IssueBookDBContext> context) : base(context)
        {
        }
             protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IssueBookEntity>()
                .HasKey(e => new { e.MemberId, e.BookId }); // Composite primary key

            // ... other model configuration
        }
        public DbSet<IssueBookEntity> IssueBookEntity { get; set; }
    }
}
