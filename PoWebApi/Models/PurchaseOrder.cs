using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Description { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; } = "NEW";
// how to set up decimal types
        [Required, Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; } = 0.00m;

        [Required]
        public bool Active { get; set; } = true;

        [Required]
        public int EmployeeId { get; set; }

        public virtual Employee employee { get; set; } // virtual allows this to not be a column in the table 

        public PurchaseOrder() // default constructor 
        {

        }

    }
}
