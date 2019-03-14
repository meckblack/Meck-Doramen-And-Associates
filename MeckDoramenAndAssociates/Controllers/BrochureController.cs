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

namespace MeckDoramenAndAssociates.Controllers
{
    public class BrochureController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public BrochureController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("brochure/add")]
        public async Task<IActionResult> Create()
        {
            #region Checker

            ViewData["CanManageLandingDetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["CanManageNews"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageNews == true);
            ViewData["CanMangeUsers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanMangeUsers == true);
            ViewData["CanManageServices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageServices == true);
            ViewData["CanManageMarketResearch"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageMarketResearch == true);
            ViewData["CanManageAboutUs"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageEnquiry"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquiry == true);
            ViewData["CanManagePartner"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePartner == true);

            #endregion

            var counter = _database.Brochure.Count();

            if (counter == 1)
            {
                TempData["brochure"] = "Sorry there exist a Brochure. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        [Route("brochure/add")]
        public async Task<IActionResult> Create(Brochure brochure, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Brochure");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    brochure.Image = filename;
                    brochure.DateCreated = DateTime.Now;
                    brochure.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    brochure.DateLastModified = DateTime.Now;
                    brochure.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    await _database.Brochure.AddAsync(brochure);
                    await _database.SaveChangesAsync();

                    TempData["brochure"] = "You have successfully added the contact details";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    return RedirectToAction("Index", "Landing");
                }
            }

            TempData["brochure"] = "Sorry you encountered an error, Try adding the brochure again";
            TempData["notificationType"] = NotificationType.Success.ToString();
            return View("Index");
        }

        #endregion

        #region Delete

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var brochure = await _database.Brochure
                .SingleOrDefaultAsync(m => m.BrochureId == id);
            if (brochure == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View("Delete");
        }

        // POST: Brochure/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var brochure = await _database.Brochure.SingleOrDefaultAsync(m => m.BrochureId == id);
            if (brochure != null)
            {
                _database.Brochure.Remove(brochure);
                await _database.SaveChangesAsync();

                TempData["brochure"] = "You have successfully deleted Meck Doramen And Associates Brochure!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View Brochure

        [HttpGet]
        [SessionExpireFilter]
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

            var brochure = await _database.Brochure.SingleOrDefaultAsync(l => l.BrochureId == id);

            if (brochure == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(brochure);
        }

        #endregion

        #region Download

        [Route("brochure/download")]
        public FileResult Download()
        {
            var _brochure = _database.Brochure.Single();
            var path = "UploadedFiles\\Brochure\\" + _brochure.Image;
            return File(path, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion

        #region Exist

        private bool BrochureExists(int id)
        {
            return _database.Brochure.Any(e => e.BrochureId == id);
        }

        #endregion


    }
}