using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required, StringLength(30)]
        public string Name { get; set; }

        [Required, Column(TypeName = "decimal(9,2)" )]
        public decimal Price { get; set; }
        [Required]
        public bool Active { get; set; } = true;
    }
}
