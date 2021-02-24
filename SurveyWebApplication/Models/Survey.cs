using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Models
{
    public class Survey
    {
        public int Id { get; set; }
        [Display(Name = "Başlık")]
        public string Header { get; set; }
        [Display(Name = "Detaylar")]
        public string Details { get; set; }
        [Display(Name = "Gerekli Onay Sayısı")]
        public int NumberOfApprovingRequired { get; set; }
        [Display(Name = "Son Oylanma Tarihi")]
        public DateTime Deadline { get; set; }
        [Display(Name = "Anket Kodu")]
        public string Code { get; set; }
        [Display(Name = "Evet Sayısı")]
        public int? NumberOfYes { get; set; }
        [Display(Name = "Hayır Sayısı")]
        public int? NumberOfNo { get; set; }

        [Display(Name = "Yorumlar")]
        public IList<Comment> Comments { get; set; }
        public IList<UserSurvey> Users { get; set; }
    }
}
