namespace Libaray_Management_System.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }

        public string UId { get; set; }

       /* public string DocumentType { get; set; }
*/
        public int Version { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool Active { get; set; }

        public bool Archived { get; set; }

        public void Initialize(bool isNew, string createdOrUpdatedBy)
        {
            Id = Guid.NewGuid().ToString();
            Active = true;
            Archived = false;


            if (isNew)
            {
                // Adding new record
                UId = Id;
                CreatedBy = createdOrUpdatedBy;
                CreatedOn = DateTime.UtcNow;
                Version = 1;
                UpdatedBy = createdOrUpdatedBy;
                UpdatedOn = CreatedOn;
            }
            else
            {
                //updating the record
                UpdatedBy = createdOrUpdatedBy;
                UpdatedOn = DateTime.UtcNow;
                Version++;
            }

        }
    }
}
