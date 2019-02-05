using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EnquiryController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public EnquiryController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index()
        {
            var userObject = _session.GetString("MDnAloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;
            var role = await _database.Roles.FindAsync(roleid);
            ViewData["userrole"] = role.Name;

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == _user.RoleId);

            var _enquiry = await _database.Enquiry.ToListAsync();
            return View(_enquiry);
        }

        #endregion

        #region Make Enquiry

        [HttpPost]
        public async Task<IActionResult> MakeEnquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                var _enquiry = new Enquiry()
                {
                    FullName = enquiry.FullName,
                    Email = enquiry.Email,
                    Message = enquiry.Message,
                    PhoneNumber = enquiry.PhoneNumber
                };

                await _database.Enquiry.AddAsync(_enquiry);
                await _database.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home", enquiry);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(eq => eq.EnquiryId == id);

            if (_enquiry == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Details", _enquiry);
        }

        #endregion

        #region Delete

        [HttpGet]
        [Route("enquiry/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(s => s.EnquiryId == id);

            if (_enquiry == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _enquiry);
        }

        [HttpPost]
        [Route("enquiry/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(s => s.EnquiryId == id);

            if (_enquiry != null)
            {
                _database.Enquiry.Remove(_enquiry);
                await _database.SaveChangesAsync();

                TempData["enquiry"] = "You have successfully deleted Meck Doramen And Associates Enquiry!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Enquiry");
        }

        #endregion
    }
}