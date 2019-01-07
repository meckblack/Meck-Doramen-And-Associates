﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Models.Enums;
using MeckDoramenAndAssociates.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public NewsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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

            var _news = await _database.News.ToListAsync();
            return View(_news);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("news/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create()
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return View();
        }

        [HttpPost]
        [Route("news/create")]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["news"] = "You changes where not saved, because you did not select an image file !!!";
                TempData["notificationType"] = NotificationType.Error.ToString();
                return RedirectToAction("Index", "News");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\News");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    news.Image = filename;
                    news.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    news.DateCreated = DateTime.Now;
                    news.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    news.DateLastModified = DateTime.Now;

                    TempData["news"] = "You have successfully added Meck Doramen And Associates's News !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.News.AddAsync(news);
                    await _database.SaveChangesAsync();

                    return RedirectToAction("Index", "News");
                }
            }
            TempData["news"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return View(news);
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _news = await _database.News.SingleOrDefaultAsync(s => s.NewsId == id);

            if (_news == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Edit", _news);
        }


        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(News news, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["news"] = "You changes where not saved, because you did not select an image file !!!";
                TempData["notificationType"] = NotificationType.Error.ToString();
                return RedirectToAction("Index", "News");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\News");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        news.Image = filename;
                        news.DateLastModified = DateTime.Now;
                        news.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                        TempData["news"] = "You have successfully modified Meck Doramen And Associates's News !!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Update(news);
                        await _database.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NewsExists(news.NewsId))
                        {
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "News");
                }
            }
            TempData["news"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index", "News");
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

            var _new = await _database.News.SingleOrDefaultAsync(s => s.NewsId == id);

            if (_new == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _new);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _new = await _database.News.SingleOrDefaultAsync(s => s.NewsId == id);

            if (_new != null)
            {
                _database.News.Remove(_new);
                await _database.SaveChangesAsync();

                TempData["news"] = "You have successfully deleted Meck Doramen And Associates News!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "News");
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

            var _news = await _database.News.SingleOrDefaultAsync(s => s.NewsId == id);

            if (_news == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Details", _news);
        }

        #endregion
        
        #region News Exists

        private bool NewsExists(int id)
        {
            return _database.News.Any(e => e.NewsId == id);
        }

        #endregion
    }
}