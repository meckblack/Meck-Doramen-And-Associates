using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AccountController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region SignIn

        [HttpGet]
        [Route("account/signin")]
        public async Task<IActionResult> SignIn()
        {
            var user = await _database.ApplicationUsers.CountAsync();
            if (user > 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("FirstRegistration");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(ApplicationUser user)
        {
            var _user = await _database.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (_user == null)
            {
                ViewData["mismatch"] = "Email and Password do not match";
            }

            if (_user != null)
            {
                var password = BCrypt.Net.BCrypt.Verify(user.Password, _user.Password);
                if (password == true)
                {
                    _session.SetString("imouloggedinuser", JsonConvert.SerializeObject(_user));
                    _session.SetInt32("imouloggedinuserid", _user.ApplicationUserId);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ViewData["mismatch"] = "Email and Password do not match";
                }
            }
            return View(user);
        }

        #endregion

        #region First Registeration

        [HttpGet]
        [Route("account/firstregistration")]
        public IActionResult FirstRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FirstRegistration(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var _role = new Role()
                {
                    Name = "SuperUser",
                    CanDoEverything = true,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                };

                await _database.AddAsync(_role);
                await _database.SaveChangesAsync();

                var _user = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(user.Password),
                    ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(user.ConfirmPassword),
                    CreatedBy = 1,
                    LastModifiedBy = 1,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    Role = _role,
                    RoleId = _role.RoleId,
                };

                await _database.AddAsync(_user);
                await _database.SaveChangesAsync();

                return RedirectToAction("SignIn");
            }

            return View(user);
        }

        #endregion

        #region SignOut

        [HttpGet]
        [Route("account/signout")]
        public IActionResult SignOut()
        {
            _session.Clear();
            _database.Dispose();
            return RedirectToAction("SignIn", "Account");
        }

        #endregion
        
    }
}