using System;
using System.Collections.Generic;
using System.Linq;
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
        //private readonly SurveyWebApplicationDbContext _context;

        public UsersController(IUserService userService/*SurveyWebApplicationDbContext context*/)
        {
            this.userService = userService;
            //_context = context;
        }

        public IActionResult Index()
        {
            var products = userService.GetUsers();
            return View(products);
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

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                userService.AddUser(user);
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


        public IActionResult Edit(int id)
        {
            var existingUser = userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            return View(existingUser);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                int affectedRowsCount = userService.EditUser(user);
                string message = affectedRowsCount > 0 ? $"{user.Name} admini güncellendi." : "Ne yazık ki bir problem oluştu.";

                return Json(message);
            }
            return View();

        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User admin = userService.GetUserById(id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        //// GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Users.ToListAsync());
        //}

        //// GET: Users/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// GET: Users/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Users/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Name,Username,Password,Email")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(user);
        //}

        //// POST: Users/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Username,Password,Email")] User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(user);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserExists(user.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        //// GET: Users/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var user = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(user);
        //}

        //// POST: Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var user = await _context.Users.FindAsync(id);
        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}
