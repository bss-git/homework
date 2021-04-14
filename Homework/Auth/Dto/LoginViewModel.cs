using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Auth.Dto
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указано имя для входа")]
        public string Login{ get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }
    }
}
