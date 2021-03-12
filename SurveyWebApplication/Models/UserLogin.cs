using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Models
{
    public class UserLogin
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı adı boş olamaz")]
        public string UserName { get; set; }
        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre boş olamaz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
