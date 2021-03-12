using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
//using Microsoft.Office.Interop.Word;

namespace SurveyWebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SurveysController : Controller
    {
        private ISurveyService surveyService;
        private IUserService userService;
        private IUserSurveyService userSurveyService;

        public SurveysController(ISurveyService surveyService, IUserService userService, IUserSurveyService userSurveyService)
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

        // GET: Surveys/Create
        public IActionResult Create()
        {
            return View();
        }


        // GET: Surveys/Create
        [HttpPost]
        public IActionResult Create(Survey survey)
        {
            if (ModelState.IsValid)
            {
                var username = User.FindFirstValue(ClaimTypes.Name);
                User user = userService.GetUserByUsername(username);
                surveyService.AddSurvey(survey, user);
                userSurveyService.AddUserSurvey(user, survey);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Surveys/Edit
        public IActionResult Edit(int id)
        {
            var existingSurvey = surveyService.GetSurveyById(id);
            if (existingSurvey == null)
            {
                return NotFound();
            }
            return View(existingSurvey);
        }

        // GET: Surveys/Edit/id
        [HttpPost]
        public IActionResult Edit(Survey survey)
        {
            if (ModelState.IsValid)
            {
                surveyService.EditSurvey(survey);
                return View(survey);
            }
            return View();

        }

        // GET: Surveys/Details/id
        [AllowAnonymous]
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
            ViewBag.Comments = surveyService.GetComments(survey);
            return View(survey);
        }

        // GET: Surveys/Delete/id
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            surveyService.DeleteSurvey(surveyService.GetSurveyById(id));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DownloadPdf(int id)
        {
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderUrlAsPdf("https://localhost:44393/Surveys/Details/" + id.ToString());
            var OutputPath = surveyService.GetSurveyById(id).Header + ".pdf";
            PDF.SaveAs("C:\\Users\\pelsi\\OneDrive\\Masaüstü\\" + OutputPath);
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}

