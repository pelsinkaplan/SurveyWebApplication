using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Display(Name = "Düşünceleriniz")]
        public string CommentString { get; set; }

        public int SurveyId { get; set; }
        public Survey Survey { get; set; }

    }
}
