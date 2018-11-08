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
    public class VisionController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public VisionController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var vision = new Vision();
            return PartialView("Create", vision);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Vision vision)
        {
            if (ModelState.IsValid)
            {
                vision.DateCreated = DateTime.Now;
                vision.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                vision.DateLastModified = DateTime.Now;
                vision.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                await _database.Vision.AddAsync(vision);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully added the company vision";
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

            var vision = await _database.Vision
                .FirstOrDefaultAsync(m => m.VisionId == id);
            if (vision == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", vision);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vision = await _database.Vision.FindAsync(id);
            if (vision != null)
            {
                _database.Vision.Remove(vision);
                await _database.SaveChangesAsync();
                TempData["landing"] = "You have successfully deleted the company vision";
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

            var vision = await _database.Vision
                .FirstOrDefaultAsync(m => m.VisionId == id);
            if (vision == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", vision);
        }

        #endregion
    }
}