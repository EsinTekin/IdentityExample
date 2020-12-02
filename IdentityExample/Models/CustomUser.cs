using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample.Models
{
    public class CustomUser : IdentityUser
    {
        [Display(Name = "Adı & Soyadı")]
        public string Fullname { get; set; }
        
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Cinsiyet")]

        public string  Gender { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.DateTime)]

        public DateTime DateOfBirth { get; set; }



    }
}
