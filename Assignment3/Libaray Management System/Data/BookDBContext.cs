using Libaray_Management_System.Entities;
using Microsoft.EntityFrameworkCore;

namespace Libaray_Management_System.Data
{
    public class BookDBContext :DbContext
    {
        public BookDBContext(DbContextOptions<BookDBContext> context) : base(context)
        {
        }
        public DbSet<BookEntity> BookEntity { get; set; }
        
    }
}
