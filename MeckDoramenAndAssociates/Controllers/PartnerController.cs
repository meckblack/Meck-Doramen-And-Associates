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
    public class PartnerController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public PartnerController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
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
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManagePartner == false)
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
            ViewData["CanManagePartner"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePartner == true);

            #endregion

            var _partners = await _database.Partners.ToListAsync();
            return View(_partners);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("partner/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create()
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManagePartner == false)
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
            ViewData["CanManagePartner"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePartner == true);

            #endregion

            return View();
        }

        [HttpPost]
        [Route("partner/create")]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partner partner, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["partner"] = "You changes where not saved, because you did not select an image file !!!";
                TempData["notificationType"] = NotificationType.Error.ToString();
                return RedirectToAction("Index", "Partner");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Partners");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    partner.Image = filename;
                    partner.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    partner.DateCreated = DateTime.Now;
                    partner.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    partner.DateLastModified = DateTime.Now;

                    TempData["partner"] = "You have successfully added Meck Doramen And Associates's Partner !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Partners.AddAsync(partner);
                    await _database.SaveChangesAsync();

                    return RedirectToAction("Index", "Partner");
                }
            }

            TempData["partner"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManagePartner == false)
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
            ViewData["CanManagePartner"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePartner == true);

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _partner = await _database.Partners.SingleOrDefaultAsync(s => s.PartnerId == id);

            if (_partner == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Edit", _partner);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(Partner partner, IFormFile file)
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
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Partners");
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
                        partner.Image = filename;
                        partner.DateLastModified = DateTime.Now;
                        partner.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                        TempData["partner"] = "You have successfully modified Meck Doramen And Associates's Partner !!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Update(partner);
                        await _database.SaveChangesAsync();

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PartnerExists(partner.PartnerId))
                        {
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Partner");
                }
            }

            TempData["partner"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index", "Partner");
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManagePartner == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _partner = await _database.Partners.SingleOrDefaultAsync(s => s.PartnerId == id);

            if (_partner == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _partner);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _partner = await _database.Partners.SingleOrDefaultAsync(s => s.PartnerId == id);

            if (_partner != null)
            {
                _database.Partners.Remove(_partner);
                await _database.SaveChangesAsync();

                TempData["partner"] = "You have successfully deleted Meck Doramen And Associates Parnter!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Partner");
        }

        #endregion

        #region Views

        [HttpGet]
        public async Task<IActionResult> View(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManagePartner == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _partner = await _database.Partners.SingleOrDefaultAsync(s => s.PartnerId == id);

            if (_partner == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", _partner);
        }

        #endregion

        #region Partner Exists

        private bool PartnerExists(int id)
        {
            return _database.Partners.Any(e => e.PartnerId == id);
        }

        #endregion
    }
}