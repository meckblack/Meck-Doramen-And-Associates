using System;
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
    public class BulletPointsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public BulletPointsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("bulletpoints/index/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            ViewData["CanManageLandingDetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageNews"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanMangeUsers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageServices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageMarketResearch"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageAboutUs"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageEnquiry"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);

            #endregion

            _session.SetInt32("paragraphid", Convert.ToInt32(id));

            var _bulletPoints = await _database.BulletPoints.Where(b => b.ParagraphId == id).ToListAsync();
            return View(_bulletPoints);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("bulletpoint/create")]
        public async Task<IActionResult> Create()
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            var paragraphid = _session.GetInt32("paragraphid");
            ViewBag.Paragraph = new SelectList(_database.Paragraphs.Where(p => p.ParagraphId == paragraphid), "ParagraphId", "Body");

            var bulletPoint = new BulletPoint();
            return PartialView("Create", bulletPoint);
        }

        [HttpPost]
        [Route("bulletpoint/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create(BulletPoint bulletPoint)
        {
            if (ModelState.IsValid)
            {
                bulletPoint.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                bulletPoint.DateCreated = DateTime.Now;
                bulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                bulletPoint.DateLastModified = DateTime.Now;
                
                await _database.BulletPoints.AddAsync(bulletPoint);
                await _database.SaveChangesAsync();


                TempData["bulletpoint"] = "You have successfully added a Meck Doramen And Associates's Bullet Point !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            var paragraphid = _session.GetInt32("paragraphid");
            ViewBag.Paragraph = new SelectList(_database.Paragraphs.Where(p => p.ParagraphId == paragraphid), "ParagraphId", "Name", bulletPoint.ParagraphId);
            return View(bulletPoint);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _bulletpoint = await _database.BulletPoints.SingleOrDefaultAsync(p => p.BulletPointId == id);

            if (_bulletpoint == null)
            {
                return RedirectToAction("Index", "Edit");
            }

            var paragraphid = _session.GetInt32("paragraphid");
            ViewBag.Paragraph = new SelectList(_database.Paragraphs.Where(p => p.ParagraphId == paragraphid), "ParagraphId", "Body");

            return PartialView("Edit", _bulletpoint);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id, BulletPoint bulletPoint)
        {
            if (id != bulletPoint.BulletPointId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bulletPoint.DateLastModified = DateTime.Now;
                    bulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.BulletPoints.Update(bulletPoint);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BulletPointsExists(bulletPoint.BulletPointId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["bulletpoint"] = "You have successfully modified Meck Doramen And Associates's Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            var paragraphid = _session.GetInt32("paragraphid");
            ViewBag.Paragraph = new SelectList(_database.Paragraphs.Where(p => p.ParagraphId == paragraphid), "ParagraphId", "Name", bulletPoint.ParagraphId);
            return View(bulletPoint);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _bulletpoint = await _database.BulletPoints.SingleOrDefaultAsync(s => s.BulletPointId == id);

            if (_bulletpoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var paragraphid = _bulletpoint.ParagraphId;
            var paragraph = await _database.Paragraphs.FindAsync(paragraphid);
            ViewData["paragraphbody"] = paragraph.Body;

            return PartialView("Details", _bulletpoint);
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

            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _bulletPoint = await _database.BulletPoints.SingleOrDefaultAsync(s => s.BulletPointId == id);

            if (_bulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _bulletPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _bulletPoint = await _database.BulletPoints.SingleOrDefaultAsync(s => s.BulletPointId == id);

            if (_bulletPoint != null)
            {
                _database.BulletPoints.Remove(_bulletPoint);
                await _database.SaveChangesAsync();

                TempData["bulletpoint"] = "You have successfully deleted Meck Doramen And Associates Bullet point!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Paragraph");
        }

        #endregion

        #region Bulletpoints Exists

        private bool BulletPointsExists(int id)
        {
            return _database.BulletPoints.Any(e => e.BulletPointId == id);
        }

        #endregion
    }
}