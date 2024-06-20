namespace Libaray_Management_System.Entities
{
    public class MemberEntity : BaseEntity
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
    }
}
