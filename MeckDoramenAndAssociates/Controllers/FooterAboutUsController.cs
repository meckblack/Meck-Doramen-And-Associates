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

namespace MeckDoramenAndAssociates.Controllers
{
    public class FooterAboutUsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public FooterAboutUsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            var footerAboutUs = new FooterAboutUs();
            return PartialView("Create", footerAboutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(FooterAboutUs footerAboutUs)
        {
            if (ModelState.IsValid)
            {
                footerAboutUs.DateCreated = DateTime.Now;
                footerAboutUs.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                footerAboutUs.DateLastModified = DateTime.Now;
                footerAboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                await _database.FooterAboutUs.AddAsync(footerAboutUs);
                await _database.SaveChangesAsync();

                TempData["footerAboutUs"] = "You have successfully added the Footer About Us details";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Delete

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _footerAboutUs = await _database.FooterAboutUs
                .FirstOrDefaultAsync(m => m.FooterAboutUsId == id);
            if (_footerAboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _footerAboutUs);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _footerAboutUs = await _database.FooterAboutUs.FindAsync(id);
            if (_footerAboutUs != null)
            {
                _database.FooterAboutUs.Remove(_footerAboutUs);
                await _database.SaveChangesAsync();
                TempData["footerAboutUs"] = "You have successfully deleted the Footer About Us details";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View

        // GET: Contact/View/5
        public async Task<IActionResult> View(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _footerAboutUs = await _database.FooterAboutUs
                .FirstOrDefaultAsync(m => m.FooterAboutUsId == id);
            if (_footerAboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", _footerAboutUs);
        }

        #endregion
    }
}