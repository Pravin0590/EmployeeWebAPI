using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public int Age { get; set; }

        public List<AddressDto> Addresses { get; set; }
    }
}
