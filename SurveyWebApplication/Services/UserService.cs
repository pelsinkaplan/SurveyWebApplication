using Microsoft.EntityFrameworkCore;
using SurveyWebApplication.Data;
using SurveyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Services
{
    public class UserService : IUserService
    {
        private SurveyDbContext dbContext;

        public UserService(SurveyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public List<User> GetUsers()
        {
            return dbContext.Users.AsNoTracking().ToList();
        }

        public User GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            foreach (User user in GetUsers())
            {
                if (user.Username == username)
                    return user;
            }
            return null;
        }

        public User ValidUser(string username, string password)
        {
            List<User> users = GetUsers();
            User user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            return user;
        }

        public IList<Survey> GetCurrentUsersSurveys(IList<Survey> allSurveys, int id)
        {
            List<Survey> surveys = GetUsersSurveys(id);
            return surveys;
        }

        public bool IsThereUser(string username)
        {
            foreach (User user in GetUsers())
            {
                if (user.Username == username)
                    return true;
            }
            return false;
        }

        public int JoinSurvey(User user, Survey survey)
        {
            UserSurveyService userSurveyService = new UserSurveyService(dbContext);
            userSurveyService.AddUserSurvey(user, survey);
            return dbContext.SaveChanges(); ;
        }

        public int EditUser(User user)
        {
            dbContext.Entry(user).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public List<Survey> GetUsersSurveys(int userId)
        {
            List<Survey> surveys = new List<Survey>();
            foreach (var userSurvey in dbContext.UserSurveys)
            {
                if (userSurvey.UserId == userId)
                {
                    SurveyService surveyService = new SurveyService(dbContext);
                    var survey = surveyService.GetSurveyById(userSurvey.SurveyId);
                    surveys.Add(survey);
                }
            }
            return surveys;
        }
    }
}
