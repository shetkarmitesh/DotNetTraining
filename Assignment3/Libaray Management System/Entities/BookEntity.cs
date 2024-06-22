using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Libaray_Management_System.Entities
{
    public class BookEntity:BaseEntity
    {
        [Required]

        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public DateTime PublishedDate { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public int Price { get; set; } = 0;
        public Status BookStatus { get; set; }
    }
  
}
