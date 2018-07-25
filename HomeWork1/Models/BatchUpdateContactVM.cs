using HomeWork1.Models.InputValidate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HomeWork1.Models
{
    public class BatchUpdateContactVM
    {
        public int Id { get; set; }

        [Required]
        public string 職稱 { get; set; }

        [Required]
        [PhoneNumber]
        public string 手機 { get; set; }

        [Required]
        public string 電話 { get; set; }
    }
}