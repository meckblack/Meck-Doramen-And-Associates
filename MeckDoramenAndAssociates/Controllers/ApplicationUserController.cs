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
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ApplicationUserController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            var _appUsers = await _database.ApplicationUsers.ToListAsync();
            return View(_appUsers);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("applicationuser/create")]
        public IActionResult Create()
        {
            ViewBag.Role = new SelectList(_database.Roles, "RoleId", "Name");

            var appUser = new ApplicationUser();
            return PartialView("Create", appUser);
        }

        [HttpPost]
        [Route("applicationuser/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                applicationUser.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                applicationUser.DateCreated = DateTime.Now;
                applicationUser.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                applicationUser.DateLastModified = DateTime.Now;
                applicationUser.Password = BCrypt.Net.BCrypt.HashPassword(applicationUser.Password);
                applicationUser.ConfirmPassword = BCrypt.Net.BCrypt.HashPassword(applicationUser.ConfirmPassword);

                await _database.ApplicationUsers.AddAsync(applicationUser);
                await _database.SaveChangesAsync();


                TempData["applicationuser"] = "You have successfully added a Meck Doramen And Associates's Apllication User!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            ViewBag.Role = new SelectList(_database.Roles, "RoleId", "Name", applicationUser.RoleId);
            return View(applicationUser);
        }

        #endregion

        #region Details

        [HttpGet]
        [Route("applicationuser/details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _applicationUser = await _database.ApplicationUsers.SingleOrDefaultAsync(s => s.ApplicationUserId == id);

            if (_applicationUser == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var roleid = _applicationUser.RoleId;
            var role = await _database.Roles.FindAsync(roleid);
            ViewData["rolename"] = role.Name;

            return PartialView("Details", _applicationUser);
        }

        #endregion

        #region Delete

        [HttpGet]
        [Route("applicationuser/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _applicationUser = await _database.Roles.SingleOrDefaultAsync(s => s.RoleId == id);

            if (_applicationUser == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _applicationUser);
        }

        [HttpPost]
        [Route("applicationuser/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var _applicationUser = await _database.Roles.SingleOrDefaultAsync(s => s.RoleId == id);

            if (_applicationUser != null)
            {
                _database.Roles.Remove(_applicationUser);
                await _database.SaveChangesAsync();

                TempData["applicationuser"] = "You have successfully deleted Meck Doramen And Associates Application User!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "ApplicationUser");
        }

        #endregion
    }
}