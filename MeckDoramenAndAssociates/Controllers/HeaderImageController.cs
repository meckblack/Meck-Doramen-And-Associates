using System;
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
    public class HeaderImageController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public HeaderImageController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Add Header Images

        [HttpGet]
        [Route("headerimage/add")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AddImages()
        {
            var counter = _database.Logo.Count();

            if (counter == 1)
            {
                TempData["landing"] = "Sorry there exist header image. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

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
        [SessionExpireFilterAttribute]
        [Route("headerimage/add")]
        public async Task<IActionResult> AddImages(HeaderImage image, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\HeaderImage");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    image.Image = filename;
                    image.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuser"));
                    image.DateCreated = DateTime.Now;
                    image.DateLastModified = DateTime.Now;
                    image.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuser"));

                    TempData["landing"] = "You have successfully added a Header Image !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.HeaderImages.AddAsync(image);
                    await _database.SaveChangesAsync();



                    return RedirectToAction("Index", "Landing");

                }
            }
            return View(image);
        }

        #endregion

        #region Delete Logo

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Delete(int? id)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var headerImage = await _database.HeaderImages
                .SingleOrDefaultAsync(m => m.HeaderImageId == id);
            if (headerImage == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View("Delete");
        }

        // POST: HeaderImage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var headerImage = await _database.Logo.SingleOrDefaultAsync(m => m.LogoId == id);
            if (headerImage != null)
            {
                _database.Logo.Remove(headerImage);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted Meck Doramen And Associates Header Image!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region View Logo

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> View(int? id)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            var roleid = _user.RoleId;
            var role = _database.Roles.Find(roleid);

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _headerImage = await _database.HeaderImages.SingleOrDefaultAsync(l => l.HeaderImageId == id);

            if (_headerImage == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(_headerImage);
        }

        #endregion

        #region Exist

        private bool HeaderImageExists(int id)
        {
            return _database.HeaderImages.Any(e => e.HeaderImageId == id);
        }

        #endregion
    }
}