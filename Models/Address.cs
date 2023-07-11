using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string City { get; set; }
        public string ZipCode { get; set; }

        [MaxLength(100)]
        public string State { get; set; }
        
        public int EmployeeId { get; set; } 
        public virtual Employee Employee { get; set; }  
    }
}
