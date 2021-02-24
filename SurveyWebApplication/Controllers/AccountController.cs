using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SurveyWebApplication.Models;
using SurveyWebApplication.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SurveyWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private IUserService userService;

        public IActionResult Index()
        {
            return View();
        }

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLogin userLogin, string returnUrl)
        {
            var user = userService.ValidUser(userLogin.UserName, userLogin.Password);
            if (user != null)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));
                int i = user.RoleId;
                if (i == 1)
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                else if (i == 2)
                    claims.Add(new Claim(ClaimTypes.Role, "User"));

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return Redirect("/");
            }


            ModelState.AddModelError("hata", "Kullanıcı ya da şifre yanlış");
            return View();

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        public async Task<IActionResult> Create()
        {

            List<Role> roles = new List<Role>();
            Role admin = new Role { Id = 1, Name = "Admin" };
            Role user = new Role { Id = 2, Name = "Kullanıcı" };
            roles.Add(admin);
            roles.Add(user);
            List<SelectListItem> selectListItems = getRolesForSelect();
            ViewBag.Items = selectListItems;
            return View();
        }

        // GET: Surveys/Create
        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                userService.AddUser(user);
                return RedirectToAction(nameof(Login));
            }
            return View();
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
