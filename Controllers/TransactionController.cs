using Microsoft.EntityFrameworkCore;
using LoginReg.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
namespace LoginReg.Controllers
{
    [Route("Transaction")]
    public class TransactionController : Controller
    {
        private LoginsContext _context;
    
        public TransactionController(LoginsContext context)
        {
            _context = context;
        }
        [HttpPost("SubmitTrans")]
        public IActionResult SubmitTrans(Transaction NewTrans)
        {
            if(ModelState.IsValid)
            {
                _context.Transactions.Add(NewTrans);
                _context.SaveChanges();
            }
            return RedirectToAction("Account");
        }

        [HttpGet("Account")]
        public IActionResult Account()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return RedirectToAction("Index");
            }
            User CurrentUser = _context.Users.SingleOrDefault(user => user.id == UserId);
            ViewBag.CurrentUser = CurrentUser;
            var UserTransactions = _context.Transactions
                .Where(t => t.user_id == CurrentUser.id);
            return View("Account", UserTransactions);
        }
    }
}