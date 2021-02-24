using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Models
{
    public class UserSurvey
    {
            public int UserId { get; set; }
            public User User { get; set; }

            public int SurveyId { get; set; }
            public Survey Survey { get; set; }
    }
}
