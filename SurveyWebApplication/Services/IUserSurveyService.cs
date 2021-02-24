using SurveyWebApplication.Models;
using System.Collections.Generic;

namespace SurveyWebApplication.Services
{
    public interface IUserSurveyService
    {
        void AddUserSurvey(User user, Survey survey);
        List<User> GetSurveysUsers(int surveyId);
    }
}