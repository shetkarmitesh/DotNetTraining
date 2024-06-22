using AutoMapper.Execution;
using Libaray_Management_System.Models;
using System.ComponentModel.DataAnnotations;

namespace Libaray_Management_System.Entities
{
    public class IssueBookEntity:BaseEntity
    {

        [Required]

        public int MemberId { get; set; }
        public virtual MemberModel MemberDetails { get; set; }
        public int IssuedBookId { get; set; }
        [Required]
        public int BookId { get; set; }
        public virtual BookModel BookDetails{ get; set; }
        [Required]
        public DateTime IssuedDate { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime ActualReturnDate { get; set; }
        public Status BookStatus { get; set; }

    }
    public enum Status
    {
        Available,
        Issued,
        Reserved,
        Lost,  
        Damaged  
    }
}
