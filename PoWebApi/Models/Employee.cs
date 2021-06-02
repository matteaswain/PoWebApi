using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoWebApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required, StringLength(30) ]
        public string Login { get; set; }
        [Required, StringLength(30)]
        public string Password { get; set; }
        [Required, StringLength(30)]
        public string Firstname { get; set; }
        [Required, StringLength(30)]
        public string Lastname { get; set; }
        
        public bool IsManager { get; set; } // will default to false so can leave it 



        public Employee()
        {
            // default constructor
        }
    }
}
