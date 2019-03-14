using System;
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

namespace MeckDoramenAndAssociates.Controllers
{
    public class AboutUsBulletPointsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AboutUsBulletPointsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("aboutusbulletpoint/index/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageAboutUs == false)
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

            _session.SetInt32("aboutusparagraphid", id);
            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.Where(aup => aup.AboutUsParagraphId == id).ToListAsync();

            return View(_aboutUsBulletPoint);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutusbulletpoint/create")]
        public async Task<IActionResult> Create()
        {
            #region Checker

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageAboutUs == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            var aboutusparagraphid = _session.GetInt32("aboutusparagraphid");
            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph.Where(aup => aup.AboutUsParagraphId == aboutusparagraphid), "AboutUsParagraphId", "Body");

            var _aboutUsBulletPoint = new AboutUsBulletPoint();
            return PartialView("Create", _aboutUsBulletPoint);
        }

        [HttpPost]
        [Route("aboutusbulletpoint/create")]
        public async Task<IActionResult> Create(AboutUsBulletPoint aboutUsBulletPoint)
        {
            if (ModelState.IsValid)
            {
                aboutUsBulletPoint.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsBulletPoint.DateCreated = DateTime.Now;
                aboutUsBulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsBulletPoint.DateLastModified = DateTime.Now;

                TempData["aboutusbulletpoint"] = "You have successfully added Meck Doramen And Associates's About Us Bullet Point!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.AboutUsBulletPoint.AddAsync(aboutUsBulletPoint);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            TempData["aboutusbulletpoint"] = "Sorry you have encountered an error. Kindly try adding the About Us Bullet Point again!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();

            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            #region Checker

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageAboutUs == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.SingleOrDefaultAsync(s => s.AboutUsBulletPointId == id);

            if (_aboutUsBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var aboutusparagraphid = _session.GetInt32("aboutusparagraphid");
            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph.Where(aup => aup.AboutUsParagraphId == aboutusparagraphid), "AboutUsParagraphId", "Body");

            return PartialView("Edit", _aboutUsBulletPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AboutUsBulletPoint aboutUsBulletPoint)
        {
            if (id != aboutUsBulletPoint.AboutUsBulletPointId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aboutUsBulletPoint.DateLastModified = DateTime.Now;
                    aboutUsBulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.AboutUsBulletPoint.Update(aboutUsBulletPoint);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsBulletPointExists(aboutUsBulletPoint.AboutUsBulletPointId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["aboutusbulletpoint"] = "You have successfully modified Meck Doramen And Associates's About us Bullet Point !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            TempData["aboutusbulletpoint"] = "Sorry you have encountered an error. Kindly try editing the About Us Bullet Point again!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();

            return RedirectToAction("Index");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            #region Checker

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageAboutUs == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.SingleOrDefaultAsync(a => a.AboutUsBulletPointId == id);

            if (_aboutUsBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var aboutUsParagraphId = _aboutUsBulletPoint.AboutUsParagraphId;
            var aboutUsParagraph = await _database.AboutUsParagraph.FindAsync(aboutUsParagraphId);
            ViewData["aboutusparagraphname"] = aboutUsParagraph.Body;

            return PartialView("Details", _aboutUsBulletPoint);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            #region Checker

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageAboutUs == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.SingleOrDefaultAsync(s => s.AboutUsBulletPointId == id);

            if (_aboutUsBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _aboutUsBulletPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.SingleOrDefaultAsync(s => s.AboutUsBulletPointId == id);

            if (_aboutUsBulletPoint != null)
            {
                _database.AboutUsBulletPoint.Remove(_aboutUsBulletPoint);
                await _database.SaveChangesAsync();

                TempData["aboutusbulletpoint"] = "You have successfully deleted Meck Doramen And Associates About Us Bullet Point!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "AboutUsBulletPoints");
        }

        #endregion

        #region About Us Bullet Point Exists

        private bool AboutUsBulletPointExists(int id)
        {
            return _database.AboutUsBulletPoint.Any(e => e.AboutUsBulletPointId == id);
        }

        #endregion
    }
}