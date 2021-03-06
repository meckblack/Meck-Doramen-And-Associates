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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class AboutUsParagraphController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AboutUsParagraphController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.ToListAsync();
            return View(_aboutUsParagraph);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutusparagraph/create")]
        public IActionResult Create()
        {
            ViewBag.AboutUs = new SelectList(_database.AboutUs, "AboutUsId", "Name");

            var _aboutUsParagraph = new AboutUsParagraph();
            return PartialView("Create", _aboutUsParagraph);
        }

        [HttpPost]
        [Route("aboutusparagraph/create")]
        public async Task<IActionResult> Create(AboutUsParagraph aboutUsParagraph)
        {
            if (ModelState.IsValid)
            {
                aboutUsParagraph.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsParagraph.DateCreated = DateTime.Now;
                aboutUsParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsParagraph.DateLastModified = DateTime.Now;

                TempData["aboutusparagraph"] = "You have successfully added Meck Doramen And Associates's About Us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.AboutUsParagraph.AddAsync(aboutUsParagraph);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            ViewBag.AboutUs = new SelectList(_database.AboutUs, "AboutUsId", "Name", aboutUsParagraph.AboutUsId);
            return View(aboutUsParagraph);
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }
            ViewBag.AboutUs = new SelectList(_database.AboutUs, "AboutUsId", "Name");

            return PartialView("Edit", _aboutUsParagraph);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AboutUsParagraph aboutUsParagraph)
        {
            if (id != aboutUsParagraph.AboutUsParagraphId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aboutUsParagraph.DateLastModified = DateTime.Now;
                    aboutUsParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(aboutUsParagraph);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsParagraphExists(aboutUsParagraph.AboutUsParagraphId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["aboutusparagraph"] = "You have successfully modified Meck Doramen And Associates's About us Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View(aboutUsParagraph);
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _aboutUsParagraph);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph != null)
            {
                _database.AboutUsParagraph.Remove(_aboutUsParagraph);
                await _database.SaveChangesAsync();

                TempData["aboutusparagraph"] = "You have successfully deleted Meck Doramen And Associates About Us Paragraph!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "AboutUsParagraph");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(a => a.AboutUsParagraphId == id);

            if(_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var aboutUsId = _aboutUsParagraph.AboutUsId;
            var aboutUs = await _database.AboutUs.FindAsync(aboutUsId);
            ViewData["aboutusname"] = aboutUs.Name;

            return PartialView("Details", _aboutUsParagraph);
        }

        #endregion

        #region About Us Paragraph Exists

        private bool AboutUsParagraphExists(int id)
        {
            return _database.AboutUsParagraph.Any(e => e.AboutUsParagraphId == id);
        }

        #endregion


    }
}