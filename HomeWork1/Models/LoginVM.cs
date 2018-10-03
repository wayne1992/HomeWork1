using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeWork1.Models
{
    public class LoginVM
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public string Password { get; set; }
    }
}