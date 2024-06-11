using Newtonsoft.Json;

namespace VisitorSecurityClearanceSystem.DTOs
{
    public class OfficeDTO
    {
        [JsonProperty("uId")]
        public string UId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
