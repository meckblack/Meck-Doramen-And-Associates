﻿using System;
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
    public class LandingSkillsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public LandingSkillsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var landingSkill = new LandingSkill();
            return PartialView("Create", landingSkill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(LandingSkill landingSkill)
        {
            if (ModelState.IsValid)
            {
                landingSkill.DateCreated = DateTime.Now;
                landingSkill.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                landingSkill.DateLastModified = DateTime.Now;
                landingSkill.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                await _database.LandingSkills.AddAsync(landingSkill);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully added the landing skill";
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

            var landinSkill = await _database.LandingSkills
                .FirstOrDefaultAsync(m => m.LandingSkillId == id);
            if (landinSkill == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", landinSkill);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var landingSkill = await _database.LandingSkills.FindAsync(id);
            if (landingSkill != null)
            {
                _database.LandingSkills.Remove(landingSkill);
                await _database.SaveChangesAsync();
                TempData["landing"] = "You have successfully deleted the landing skill";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View
        
        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var landingSkill = await _database.LandingSkills
                .FirstOrDefaultAsync(m => m.LandingSkillId == id);
            if (landingSkill == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", landingSkill);
        }

        #endregion
    }
}