using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyWebApplication.Data;
using SurveyWebApplication.Models;
using SurveyWebApplication.Services;

namespace SurveyWebApplication.Controllers
{
    [Authorize(Roles = "User")]
    public class SurveysForUserController : Controller
    {
        private ISurveyService surveyService;
        private IUserService userService;
        private IUserSurveyService userSurveyService;

        public SurveysForUserController(ISurveyService surveyService, IUserService userService, IUserSurveyService userSurveyService)
        {
            this.surveyService = surveyService;
            this.userService = userService;
            this.userSurveyService = userSurveyService;
        }

        // GET: Surveys/Index
        public IActionResult Index()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            List<Survey> surveys = surveyService.GetSurveys();
            var products = userService.GetCurrentUsersSurveys(surveys, user.Id);
            return View(products);
        }

        // GET: Surveys/SurveysPage
        public IActionResult SurveysPage()
        {
            List<Survey> surveys = surveyService.GetSurveys();
            List<Survey> surveysUserCanJoin = new List<Survey>();
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            foreach (Survey survey in surveys)
            {
                if (surveyService.IsUserJoinedSurvey(survey, user))
                    if (!surveyService.DidDeadlinePass(survey))
                        surveysUserCanJoin.Add(survey);
            }
            return View(surveysUserCanJoin);
        }

        // GET: Surveys/Details/id
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Survey survey = surveyService.GetSurveyById(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // GET: Surveys/JoinSurvey/id
        public async Task<IActionResult> JoinSurvey(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Survey survey = surveyService.GetSurveyById(id);
            if (survey == null)
            {
                return NotFound();
            }
            return View(survey);
        }

        // GET: Surveys/JoinSurvey/id
        public async Task<IActionResult> Survey(string code, Survey survey, string yesNo)
        {
            Survey wantedSurvey = surveyService.GetSurveyByCode(code);
            if (surveyService.GetSurveyById(wantedSurvey.Id) == null)
            {
                return NotFound();
            }
            surveyService.AddComment(surveyService.GetSurveyById(wantedSurvey.Id), survey.Details);
            surveyService.IncreaseYesNoNum(surveyService.GetSurveyById(wantedSurvey.Id), yesNo);
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            surveyService.UserJoinSurvey(surveyService.GetSurveyById(wantedSurvey.Id), user);
            return View(wantedSurvey);
        }

        // GET: Surveys/JoinSurvey/id
        public async Task<IActionResult> SurveyCode()
        {
            return View();
        }

        // GET: Surveys/JoinSurvey/id
        [HttpPost]
        public async Task<IActionResult> SurveyCode(Survey survey)
        {
            surveyService.GetSurveyByCode(survey.Code);
            return RedirectToAction(nameof(Survey), new { code = survey.Code });
        }

        // GET: Surveys/JoinSurvey/id
        [HttpPost]
        public IActionResult JoinSurvey(Survey survey, int id, string yesNo)
        {
            if (surveyService.GetSurveyById(id) == null)
            {
                return NotFound();
            }
            surveyService.AddComment(surveyService.GetSurveyById(id), survey.Details);
            surveyService.IncreaseYesNoNum(surveyService.GetSurveyById(id), yesNo);
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            surveyService.UserJoinSurvey(surveyService.GetSurveyById(id), user);
            return View(surveyService.GetSurveyById(id));
        }
    }
}
