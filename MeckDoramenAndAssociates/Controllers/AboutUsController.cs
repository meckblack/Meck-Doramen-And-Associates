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
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AboutUsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            var _aboutUs = await _database.AboutUs.ToListAsync();
            return View(_aboutUs);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutus/create")]
        public IActionResult Create()
        {
            var _aboutUs = new AboutUs();
            return PartialView("Create", _aboutUs);
        }

        [HttpPost]
        [Route("aboutus/create")]
        public async Task<IActionResult> Create(AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
                aboutUs.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUs.DateCreated = DateTime.Now;
                aboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUs.DateLastModified = DateTime.Now;

                TempData["aboutus"] = "You have successfully added Meck Doramen And Associates's About Us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.AboutUs.AddAsync(aboutUs);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            return View(aboutUs);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _aboutUs= await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return PartialView("Edit", _aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AboutUs aboutUs)
        {
            if (id != aboutUs.AboutUsId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aboutUs.DateLastModified = DateTime.Now;
                    aboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(aboutUs);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsExists(aboutUs.AboutUsId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["abooutus"] = "You have successfully modified Meck Doramen And Associates's About us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            
            return View(aboutUs);
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

            var _aboutUs = await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _aboutUs = await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs != null)
            {
                _database.AboutUs.Remove(_aboutUs);
                await _database.SaveChangesAsync();

                TempData["aboutus"] = "You have successfully deleted Meck Doramen And Associates About Us!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "AboutUs");
        }

        #endregion
        
        #region About Us Exists

        private bool AboutUsExists(int id)
        {
            return _database.AboutUs.Any(e => e.AboutUsId == id);
        }

        #endregion
    }
}