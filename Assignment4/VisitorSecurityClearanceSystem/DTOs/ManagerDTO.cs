using Newtonsoft.Json;

namespace VisitorSecurityClearanceSystem.DTOs
{
    public class ManagerDTO
    {
       
        [JsonProperty("uId")]
        public string UId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("phone")]
        public int Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
