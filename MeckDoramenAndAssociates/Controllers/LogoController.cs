using System;
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
    public class LogoController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public LogoController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Add Logo

        [HttpGet]
        [Route("logo/add")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> AddLogo()
        {
            var counter = _database.Logo.Count();

            if (counter == 1)
            {
                TempData["landing"] = "Sorry there exist a logo. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Landing");
            }

            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            ViewData["CanManageLandingDetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["CanManageNews"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageNews == true);
            ViewData["CanMangeUsers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanMangeUsers == true);
            ViewData["CanManageServices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageServices == true);
            ViewData["CanManageMarketResearch"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageMarketResearch == true);
            ViewData["CanManageAboutUs"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageEnquiry"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquiry == true);

            #endregion

            return View();
        }

        [HttpPost]
        [SessionExpireFilterAttribute]
        [Route("logo/add")]
        public async Task<IActionResult> AddLogo(Logo logo, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Logo");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    logo.Image = filename;
                    logo.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    logo.DateCreated = DateTime.Now;
                    logo.DateLastModified = DateTime.Now;
                    logo.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    TempData["landing"] = "You have successfully added Meck Doramen And Associates's Logo !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Logo.AddAsync(logo);
                    await _database.SaveChangesAsync();



                    return RedirectToAction("Index", "Landing");

                }
            }
            return View(logo);
        }

        #endregion

        #region Delete Logo

        [HttpGet]
        [SessionExpireFilter]
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

            var logo = await _database.Logo
                .SingleOrDefaultAsync(m => m.LogoId == id);
            if (logo == null)
            {
                return RedirectToAction("Index", "Error");
            }
            return View("Delete");
        }

        // POST: Logo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var logo = await _database.Logo.SingleOrDefaultAsync(m => m.LogoId == id);
            if (logo != null)
            {
                _database.Logo.Remove(logo);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully deleted Meck Doramen And Associates Logo!";
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

            var _logo = await _database.Logo.SingleOrDefaultAsync(l => l.LogoId == id);

            if (_logo == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(_logo);
        }

        #endregion

        #region Exist

        private bool LogoExists(int id)
        {
            return _database.Logo.Any(e => e.LogoId == id);
        }

        #endregion
    }
}