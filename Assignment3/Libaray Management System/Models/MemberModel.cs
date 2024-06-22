using System.ComponentModel.DataAnnotations;

namespace Libaray_Management_System.Models
{
    public class MemberModel
    {
        [Required]
        [Key]
        public int MemberId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int PhoneNo { get; set; }

        public int Penalty {  get; set; }

    }
}
