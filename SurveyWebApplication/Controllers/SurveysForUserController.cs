using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
                if (!surveyService.IsUserJoinedSurvey(survey, user))
                    if (!surveyService.DidDeadlinePass(survey))
                        if (survey.Code == null)
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

        // GET: Surveys/JoinSurvey/survey id yesNo
        [HttpPost]
        public IActionResult JoinSurvey(Survey survey, int id, string yesNo)
        {

            if (surveyService.GetSurveyById(id) == null)
            {
                return NotFound();
            }
            if (survey.Details != null)
                surveyService.AddComment(surveyService.GetSurveyById(id), survey.Details);
            surveyService.IncreaseYesNoNum(surveyService.GetSurveyById(id), yesNo);
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            surveyService.UserJoinSurvey(surveyService.GetSurveyById(id), user);
            if (surveyService.IsRequiredVoteOkey(id))
            {
                SendEmail(surveyService.GetSurveyById(id).Header, surveyService.GetSurveyById(id).Header, surveyService.GetSurveysAdmin(id).Email);
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Surveys/JoinSurvey/id
        public async Task<IActionResult> ErrorPage(string error)
        {
            ViewBag.Error = error;
            return View();
        }


        // GET: Surveys/JoinSurvey/id
        public async Task<IActionResult> Survey(string code)
        {
            if (code == null)
                return RedirectToAction(nameof(ErrorPage), new { error = "Kodu Olmayan Anketlere Anketlere Katıl Sayfasından Ulaşabilirsiniz." });

            Survey wantedSurvey = surveyService.GetSurveyByCode(code);
            if (wantedSurvey == null)
                return RedirectToAction(nameof(ErrorPage), new { error = "Aradığınız Kodda Bir Anket Bulunmamaktadır!" });

            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            if (surveyService.IsUserJoinedSurvey(surveyService.GetSurveyByCode(code), user))

                return RedirectToAction(nameof(ErrorPage), new { error = "Bu ankete katıldınız!" });

            if (surveyService.DidDeadlinePass(surveyService.GetSurveyByCode(code)))
                return RedirectToAction(nameof(ErrorPage), new { error = "Bu anketin son oylanma tarihi geçmiş!" });

            return View(wantedSurvey);
        }

        // GET: Surveys/JoinSurvey/id
        [HttpPost]
        public async Task<IActionResult> Survey(int id, Survey survey, string yesNo)
        {
            Survey wantedSurvey = surveyService.GetSurveyById(id);

            if (survey.Details != null)
                surveyService.AddComment(surveyService.GetSurveyById(wantedSurvey.Id), survey.Details);
            surveyService.IncreaseYesNoNum(surveyService.GetSurveyById(wantedSurvey.Id), yesNo);
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);
            surveyService.UserJoinSurvey(surveyService.GetSurveyById(wantedSurvey.Id), user);
            if (surveyService.IsRequiredVoteOkey(wantedSurvey.Id))
            {
                SendEmail(surveyService.GetSurveyById(wantedSurvey.Id).Header, surveyService.GetSurveyById(wantedSurvey.Id).Header, surveyService.GetSurveysAdmin(wantedSurvey.Id).Email);
            }
            return RedirectToAction(nameof(Details), new { id = id });
        }


        // GET: Surveys/SurveyCode
        public async Task<IActionResult> SurveyCode()
        {
            return View();
        }

        // GET: Surveys/SurveyCode/survey
        [HttpPost]
        public async Task<IActionResult> SurveyCode(Survey survey)
        {
            return RedirectToAction(nameof(Survey), new { code = survey.Code });
        }

        public void SendEmail(string subject, string body, string email)
        {
            MailMessage ePosta = new MailMessage();
            ePosta.From = new MailAddress("anketim.temp@gmail.com");
            //
            ePosta.To.Add(email);
            ePosta.Subject = subject + "Anketi";
            ePosta.Body = body + " Anketi için gerekli katılım sağlandı.";
            //
            SmtpClient smtp = new SmtpClient();
            //
            smtp.Credentials = new System.Net.NetworkCredential("anketim.temp@gmail.com", "0123456789.At");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            object userState = ePosta;
            try
            {
                smtp.Send(ePosta);
            }
            catch (SmtpException ex)
            {
            }
        }
    }
}
