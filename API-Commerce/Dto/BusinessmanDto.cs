using System.ComponentModel.DataAnnotations;

namespace API_Commerce.Dto
{
    public class BusinessmanDto
    {
        [Required]
        public string Bus_Name { get; set; }

        public string? Bus_Phone_Number { get; set; }

        public string? Bus_Email { get; set; }

        [Required]
        public string Bus_Status { get; set; }

        [Required]
        public string Bus_Municipality { get; set; }
    }
}
