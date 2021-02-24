using Microsoft.EntityFrameworkCore;
using SurveyWebApplication.Data;
using SurveyWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurveyWebApplication.Services
{
    public class SurveyService : ISurveyService
    {
        private SurveyDbContext dbContext;

        public SurveyService(SurveyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddSurvey(Survey survey, User user)
        {
            dbContext.Surveys.Add(survey);
            dbContext.SaveChanges();
        }

        public List<Survey> GetSurveys()
        {
            return dbContext.Surveys.AsNoTracking().ToList();
        }

        public Survey GetSurveyById(int id)
        {
            return dbContext.Surveys.FirstOrDefault(x => x.Id == id);
        }

        public Survey GetSurveyByCode(string code)
        {
            foreach (Survey survey in GetSurveys())
            {
                if (survey.Code == code)
                    return survey;
            }
            return null;
        }

        public IList<User> GetSurveysUsers(int id)
        {
            UserSurveyService userSurveyService = new UserSurveyService(dbContext);
            return userSurveyService.GetSurveysUsers(id);
        }

        public int EditSurvey(Survey survey)
        {
            dbContext.Entry(survey).State = EntityState.Modified;

            return dbContext.SaveChanges();
        }

        public List<User> GetAdmins()
        {
            List<User> users = dbContext.Users.AsNoTracking().ToList();
            List<User> admins = new List<User>();
            foreach (User user in users)
            {
                if (user.RoleId == 1)
                {
                    admins.Add(user);
                }
            }
            return admins;
        }

        public int DeleteSurvey(Survey survey)
        {
            dbContext.Entry(survey).State = EntityState.Deleted;

            return dbContext.SaveChanges();
        }

        public int IncreaseYesNoNum(Survey survey, string yesOrNo)
        {
            if (yesOrNo == "0")
            {
                survey.NumberOfYes += 1;
                dbContext.Entry(survey).State = EntityState.Modified;
            }
            else if (yesOrNo == "1")
            {
                survey.NumberOfNo += 1;
                dbContext.Entry(survey).State = EntityState.Modified;
            }

            return dbContext.SaveChanges();
        }

        public bool IsUserJoinedSurvey(Survey survey, User user)
        {
            UserService userService = new UserService(dbContext);
            foreach (Survey usersSurvey in userService.GetUsersSurveys(user.Id))
            {
                if (survey.Header == usersSurvey.Header)
                    return false;
            }
            return true;
        }

        public bool UserJoinSurvey(Survey survey, User user)
        {
            UserService userService = new UserService(dbContext);
            userService.JoinSurvey(user, survey);
            return true;
        }

        public bool DidDeadlinePass(Survey survey)
        {
            TimeSpan time = survey.Deadline - DateTime.Now;
            if (time.TotalHours < 0)
                return true;
            return false;
        }
        public List<Comment> GetComments(Survey survey)
        {
            List<Comment> comments = new List<Comment>();
            foreach (Comment comment in dbContext.Comments)
            {
                if (comment.SurveyId == survey.Id)
                    comments.Add(comment);
            }
            return comments;
        }

        public int AddComment(Survey survey, string comment)
        {
            Comment commentObject = new Comment();
            commentObject.CommentString = comment;
            commentObject.SurveyId = survey.Id;
            commentObject.Survey = survey;
            dbContext.Comments.Add(commentObject);
            return dbContext.SaveChanges();
        }
    }
}
