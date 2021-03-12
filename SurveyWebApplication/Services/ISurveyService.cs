using SurveyWebApplication.Models;
using System.Collections.Generic;

namespace SurveyWebApplication.Services
{
    public interface ISurveyService
    {
        void AddSurvey(Survey survey, User user);
        int EditSurvey(Survey survey);
        List<User> GetAdmins();
        Survey GetSurveyById(int id);
        List<Survey> GetSurveys();
        IList<User> GetSurveysUsers(int id);
        int DeleteSurvey(Survey survey);
        int IncreaseYesNoNum(Survey survey, string yesOrNo);
        bool IsUserJoinedSurvey(Survey survey, User user);
        bool DidDeadlinePass(Survey survey);
        List<Comment> GetComments(Survey survey);
        Survey GetSurveyByCode(string code);
        int AddComment(Survey survey, string comment);
        bool UserJoinSurvey(Survey survey, User user);
        void DownloadPdf(int id);
        bool IsRequiredVoteOkey(int id);
        User GetSurveysAdmin(int id);
    }
}