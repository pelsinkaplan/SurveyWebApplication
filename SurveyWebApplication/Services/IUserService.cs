using SurveyWebApplication.Models;
using System.Collections.Generic;

namespace SurveyWebApplication.Services
{
    public interface IUserService
    {
        void AddUser(User user);
        int EditUser(User user);
        IList<Survey> GetCurrentUsersSurveys(IList<Survey> allSurveys, int id);
        User GetUserById(int id);
        User GetUserByUsername(string username);
        List<User> GetUsers();
        List<Survey> GetUsersSurveys(int userId);
        bool IsThereUser(string username);
        int JoinSurvey(User user, Survey survey);
        User ValidUser(string username, string password);
    }
}