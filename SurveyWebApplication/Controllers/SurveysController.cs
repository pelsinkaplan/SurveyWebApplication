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

        // GET: Surveys/Delete
        public async Task<IActionResult> Delete()
        {
            return View();
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
    }
}
