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
    public class LandingAboutUsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public LandingAboutUsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var landingAbooutUs = new LandingAboutUs();
            return PartialView("Create", landingAbooutUs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(LandingAboutUs landingAboutUs)
        {
            if (ModelState.IsValid)
            {
                landingAboutUs.DateCreated = DateTime.Now;
                landingAboutUs.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                landingAboutUs.DateLastModified = DateTime.Now;
                landingAboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                await _database.LandingAboutUs.AddAsync(landingAboutUs);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully added the landing about us";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var landingAboutUs = await _database.LandingAboutUs
                .FirstOrDefaultAsync(m => m.LandingAboutUsId == id);
            if (landingAboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", landingAboutUs);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _landingAboutUs = await _database.LandingAboutUs.FindAsync(id);
            if (_landingAboutUs != null)
            {
                _database.LandingAboutUs.Remove(_landingAboutUs);
                await _database.SaveChangesAsync();
                TempData["landing"] = "You have successfully deleted the landing about us";
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
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var landingAboutUs = await _database.LandingAboutUs
                .FirstOrDefaultAsync(m => m.LandingAboutUsId == id);
            if (landingAboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", landingAboutUs);
        }

        #endregion
        
    }
}