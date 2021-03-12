using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SurveyWebApplication.Data;
using SurveyWebApplication.Models;
using SurveyWebApplication.Services;

namespace SurveyWebApplication.Controllers
{
    public class UsersController : Controller
    {
        private IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Account()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Account(string userName)
        {
            if (ModelState.IsValid)
            {
                if (!userService.IsThereUser(userName))
                {
                    string message = "Ne yazık ki bir problem oluştu.";

                    return Json(message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            List<Role> roles = new List<Role>();
            Role admin = new Role { Id = 1, Name = "Admin" };
            Role user = new Role { Id = 2, Name = "Kullanıcı" };
            roles.Add(admin);
            roles.Add(user);
            List<SelectListItem> selectListItems = getRolesForSelect();
            ViewBag.Items = selectListItems;
            var existingUser = userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            return View(existingUser);
        }

        [HttpPost]
        public IActionResult Edit(User currentUser)
        {
            List<Role> roles = new List<Role>();
            Role admin = new Role { Id = 1, Name = "Admin" };
            Role user = new Role { Id = 2, Name = "Kullanıcı" };
            roles.Add(admin);
            roles.Add(user);
            List<SelectListItem> selectListItems = getRolesForSelect();
            ViewBag.Items = selectListItems;
            if (ModelState.IsValid)
            {
                int affectedRowsCount = userService.EditUser(currentUser);
                string message = affectedRowsCount > 0 ? $"{currentUser.Name} admini güncellendi." : "Ne yazık ki bir problem oluştu.";

                return RedirectToAction(nameof(Details));
            }
            return View();

        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            User user = userService.GetUserByUsername(username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        private List<SelectListItem> getRolesForSelect()
        {
            List<Role> roles = new List<Role>();
            Role admin = new Role { Id = 1, Name = "Admin" };
            Role user = new Role { Id = 2, Name = "Kullanıcı" };
            roles.Add(admin);
            roles.Add(user);
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            roles.ToList().ForEach(role => selectListItems.Add(new SelectListItem
            {
                Text = role.Name,
                Value = role.Id.ToString()
            }));
            return selectListItems;
        }

    }
}
