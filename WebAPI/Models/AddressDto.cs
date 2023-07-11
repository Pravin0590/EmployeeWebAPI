using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class AddressDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; }
        
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
