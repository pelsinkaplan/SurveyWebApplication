using SurveyWebApplication.Data;
using SurveyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Services
{
    public class UserSurveyService : IUserSurveyService
    {
        private SurveyDbContext dbContext;

        public UserSurveyService(SurveyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUserSurvey(User user, Survey survey)
        {
            UserSurvey userSurvey = new UserSurvey();
            userSurvey.SurveyId = survey.Id;
            userSurvey.UserId = user.Id;
            dbContext.UserSurveys.Add(userSurvey);
            dbContext.SaveChanges();
        }

        public List<User> GetSurveysUsers(int surveyId)
        {
            List<User> users = new List<User>();
            foreach (var userSurvey in dbContext.UserSurveys)
            {
                if (userSurvey.SurveyId == surveyId)
                {
                    UserService userService = new UserService(dbContext);
                    users.Add(userService.GetUserById(userSurvey.UserId));
                }
            }
            return users;
        }

    }
}
