using Microsoft.EntityFrameworkCore;
using LoginReg.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
namespace LoginReg.Controllers
{
    public class LoginsController : Controller
    {
        private LoginsContext _context;
    
        public LoginsController(LoginsContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ShowUser");
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(User NewUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.email == NewUser.email))
                {
                    return View("Index");
                }

                PasswordHasher<User> hasher = new PasswordHasher<User>();
                NewUser.password = hasher.HashPassword(NewUser, NewUser.password);

                ModelState.AddModelError("email", "email exists!");
                _context.Users.Add(NewUser);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", NewUser.id);
                return RedirectToAction("ShowUser");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("ShowUser");
            }
        }

        [HttpPost("LoginUser")]
        public IActionResult LoginUser(LoginUser User)
        {
            if(ModelState.IsValid)
            {
                User FoundUser = _context.Users.FirstOrDefault(u => u.email == User.email);
                if(FoundUser == null)
                {
                    ModelState.AddModelError("email", "Invalid Email/Password");
                    return View("Login");
                }

                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(User, FoundUser.password, User.password);
                if(result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("email", "Invalid Email/Password");
                    return View("Login");
                }

                HttpContext.Session.SetInt32("UserId", FoundUser.id);
                return RedirectToAction("ShowUser");
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("ShowUser")]
        public IActionResult ShowUser()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return RedirectToAction("Index");
            }
            User CurrentUser = _context.Users.SingleOrDefault(user => user.id == UserId);
            ViewBag.CurrentUser = CurrentUser;
            return RedirectToAction("Account", "Transaction");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        
        [HttpGet("AllUsers")]
        public IActionResult AllUsers()
        {
            List<User> AllUsers = _context.Users.ToList();
            ViewBag.Users = AllUsers;
            return View("AllUsers");
        }
    }
}