using System;
using System.Threading.Tasks;
using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Models.Enums;
using MeckDoramenAndAssociates.Services;
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
        //[Route("account/signin")]
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
                    _session.SetString("MDnAloggedinuser", JsonConvert.SerializeObject(_user));
                    _session.SetInt32("MDnAloggedinuserid", _user.ApplicationUserId);
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

                await _database.Roles.AddAsync(_role);
                await _database.SaveChangesAsync();

                var _user = new ApplicationUser()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = new Hashing().HashPassword(user.Password),
                    ConfirmPassword = new Hashing().HashPassword(user.ConfirmPassword),
                    CreatedBy = 1,
                    LastModifiedBy = 1,
                    DateCreated = DateTime.Now,
                    DateLastModified = DateTime.Now,
                    Role = _role,
                    RoleId = _role.RoleId,
                };

                await _database.ApplicationUsers.AddAsync(_user);
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

        #region View Profile

        [SessionExpireFilterAttribute]
        [Route("account/viewprofile")]
        public async Task<IActionResult> ViewProfile()
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");

            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == userid);

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.RoleId != roleid)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return View(_user);
        }

        #endregion

        #region Edit Profile

        [HttpGet]
        [Route("account/editprofile")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> EditProfile()
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");

            if (userid == null)
            {
                TempData["error"] = "Sorry your session has expired. Try signin again";
                return RedirectToAction("Signin", "Account");
            }

            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == userid);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.RoleId != roleid)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return PartialView("EditProfile", _user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> EditProfile(ApplicationUser applicationUser)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");

            if (userid != applicationUser.ApplicationUserId)
            {
                return RedirectToAction("Index", "Error");
            }

            try
            {
                applicationUser.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                applicationUser.DateLastModified = DateTime.Now;

                TempData["user"] = "You have successfully modified " + applicationUser.DisplayName + " profile.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                _database.ApplicationUsers.Update(applicationUser);
                await _database.SaveChangesAsync();

                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        
        #region Password Recovery

        [HttpGet]
        [Route("account/passwordrecovery")]
        public IActionResult PasswordRecovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRecovery(ApplicationUser user)
        {
            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.Email == user.Email);

            if (_user == null)
            {
                ViewData["appuser"] = "Sorry the email you enter does not exist. Check the email and try again";
                return View();
            }

            //new Mailer().PasswordRecovery(new AppConfig().ForgotPasswordHtml, _user);

            _session.SetString("recoveriedemail", _user.Email);

            return RedirectToAction("Success", "Account");

        }

        #endregion

        #region Success

        [HttpGet]
        [Route("account/success")]
        public IActionResult Success()
        {
            ViewData["recoveriedemail"] = _session.GetString("recoveriedemail");
            if (ViewData["recoveriedemail"] == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View();
        }

        #endregion

        #region New Password

        [HttpGet]
        [Route("account/newpassword")]
        public async Task<IActionResult> NewPassword(string appUser)
        {
            if (appUser == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _checker = await _database.ApplicationUsers.SingleOrDefaultAsync(c => c.Password == appUser);

            if (_checker == null)
            {
                return RedirectToAction("Index", "Error");
            }

            _session.SetInt32("id", _checker.ApplicationUserId);
            return View(_checker);
        }

        [HttpPost]
        public async Task<IActionResult> NewPassword(ApplicationUser applicationUser)
        {
            var userid = _session.GetInt32("id");

            if (userid != applicationUser.ApplicationUserId)
            {
                return RedirectToAction("Index", "Error");
            }
            try
            {
                applicationUser.Password = BCrypt.Net.BCrypt.HashPassword(applicationUser.Password);
                applicationUser.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(applicationUser.ConfirmPassword);

                
                _database.ApplicationUsers.Update(applicationUser);
                await _database.SaveChangesAsync();

                return RedirectToAction("Signin", "Account");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Change Password

        [HttpGet]
        [Route("account/changepassword")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> ChangePassword()
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");

            if (userid == null)
            {
                TempData["error"] = "Sorry your session has expired. Try signin again";
                return RedirectToAction("Signin", "Account");
            }

            var _user = await _database.ApplicationUsers.SingleOrDefaultAsync(u => u.ApplicationUserId == userid);

            if (_user == null)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.RoleId != roleid)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return View("ChangePassword", _user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> ChangePassword(ApplicationUser applicationUser)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");

            if (userid != applicationUser.ApplicationUserId)
            {
                return RedirectToAction("Index", "Error");
            }
            try
            {
                applicationUser.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(applicationUser.ConfirmPassword);
                applicationUser.Password = BCrypt.Net.BCrypt.HashPassword(applicationUser.Password);
                applicationUser.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                applicationUser.DateLastModified = DateTime.Now;

                TempData["user"] = "You have successfully changed " + applicationUser.DisplayName + " password.";
                TempData["notificationType"] = NotificationType.Success.ToString();

                _database.ApplicationUsers.Update(applicationUser);
                await _database.SaveChangesAsync();

                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}