using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class PoLine
    {
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        public int PurchaseOrderId { get; set; }

        public virtual PurchaseOrder purchaseorder { get; set; }

        [Required]
        public int ItemId { get; set; }

        public virtual Item item { get; set; }


        public PoLine() // default constructor
        {

        }
    }
}
