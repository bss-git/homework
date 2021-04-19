using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Users
{
    public enum Gender : byte
    {
        [Display(Name = "")]
        Unset = 0,
        
        [Display(Name = "Мужской")]
        Male = 1,

        [Display(Name = "Женский")]
        Female = 2
    }
}
