using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        [Display(Name = "İsim")]
        public string Name { get; set; }
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public IList<UserSurvey> Surveys { get; set; }
    }
}
