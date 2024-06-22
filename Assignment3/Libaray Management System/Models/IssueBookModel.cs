using System.ComponentModel.DataAnnotations;

namespace Libaray_Management_System.Models
{
    public class IssueBookModel
    {
        public int IssuedBookId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}
