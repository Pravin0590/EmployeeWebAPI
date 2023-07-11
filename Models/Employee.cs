using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [Required]
        [EmailAddress]
        //[RegularExpression("^[a-zA-Z0-9]@[a-zA-Z0-9].[a-zA-Z]{2,3}$", ErrorMessage = "Please Enter Valid Email Address")]
        public string EmailAddress { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}