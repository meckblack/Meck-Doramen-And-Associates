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
    public class MarketResearchBulletPointController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public MarketResearchBulletPointController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [Route("marketresearchbulletpoint/index/{id}")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageMarketResearch == false)
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

            _session.SetInt32("marketresearchparagraphid", Convert.ToInt32(id));

            var _marketresearchBulletPoint = await _database.MarketResearchBulletPoints.Where(mrb => mrb.MarketResearchParagraphId == id).ToListAsync();
            return View(_marketresearchBulletPoint);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("marketresearchbulletpoint/create")]
        public async Task<IActionResult> Create()
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

            #endregion

            var marketresearchparagraphid = _session.GetInt32("marketresearchparagraphid");
            ViewBag.MarketResearchParagraph = new SelectList(_database.MarketResearchParagraphs.Where(mrp => mrp.MarketResearchParagraphId == marketresearchparagraphid),
                                                             "MarketResearchParagraphId", "Body");

            var marketResearchBulletPoint = new MarketResearchBulletPoint();
            return PartialView("Create", marketResearchBulletPoint);
        }

        [HttpPost]
        [Route("marketresearchbulletpoint/create")]
        public async Task<IActionResult> Create(MarketResearchBulletPoint marketResearchBulletPoint)
        {
            if (ModelState.IsValid)
            {
                marketResearchBulletPoint.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketResearchBulletPoint.DateCreated = DateTime.Now;
                marketResearchBulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketResearchBulletPoint.DateLastModified = DateTime.Now;

                TempData["marketresearchbulletpoint"] = "You have successfully added Meck Doramen And Associates's Market Research Bullet Point!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.MarketResearchBulletPoints.AddAsync(marketResearchBulletPoint);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            TempData["marketresearchbulletpoint"] = "Sorry you have encountered an error. Kindly try adding the Market Research Bullet Point again!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();

            return RedirectToAction("Index");
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

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearchBulletPoint = await _database.MarketResearchBulletPoints.SingleOrDefaultAsync(s => s.MarketResearchBulletPointId == id);

            if (_marketResearchBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var marketresearchparagraphid = _session.GetInt32("marketresearchparagraphid");
            ViewBag.MarketResearchParagraph = new SelectList(_database.MarketResearchParagraphs.Where(mrp => mrp.MarketResearchParagraphId == marketresearchparagraphid),
                                                             "MarketResearchParagraphId", "Body");

            return PartialView("Edit", _marketResearchBulletPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, MarketResearchBulletPoint marketResearchBulletPoint)
        {
            if (id != marketResearchBulletPoint.MarketResearchBulletPointId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    marketResearchBulletPoint.DateLastModified = DateTime.Now;
                    marketResearchBulletPoint.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.MarketResearchBulletPoints.Update(marketResearchBulletPoint);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketResearchBulletPointExists(marketResearchBulletPoint.MarketResearchBulletPointId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["markeresearchbulletpoint"] = "You have successfully modified Meck Doramen And Associates's Market Research Bullet Point !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            TempData["markeresearchbulletpoint"] = "Sorry you have encountered an error. Kindly try editing the Market Research Bullet Point again !!!";
            TempData["notificationType"] = NotificationType.Error.ToString();

            return RedirectToAction("Index");
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

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearchBulletPoint = await _database.MarketResearchBulletPoints.SingleOrDefaultAsync(a => a.MarketResearchBulletPointId == id);

            if (_marketResearchBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var marketResearchParagraphId = _marketResearchBulletPoint.MarketResearchParagraphId;
            var marketResearchParagraph = await _database.MarketResearchParagraphs.FindAsync(marketResearchParagraphId);
            ViewData["marketresearchparagraphname"] = marketResearchParagraph.Body;

            return PartialView("Details", _marketResearchBulletPoint);
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

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearchBulletPoint = await _database.MarketResearchBulletPoints.SingleOrDefaultAsync(s => s.MarketResearchParagraphId == id);

            if (_marketResearchBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _marketResearchBulletPoint);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _marketResearchBulletPoint = await _database.MarketResearchBulletPoints.SingleOrDefaultAsync(s => s.MarketResearchBulletPointId == id);

            if (_marketResearchBulletPoint != null)
            {
                _database.MarketResearchBulletPoints.Remove(_marketResearchBulletPoint);
                await _database.SaveChangesAsync();

                TempData["marketresearchbulletpoint"] = "You have successfully deleted Meck Doramen And Associates Market Research Bullet Point!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "MarketResearchBulletPoints");
        }

        #endregion

        #region Market Research Bullet Point Exists

        private bool MarketResearchBulletPointExists(int id)
        {
            return _database.MarketResearchBulletPoints.Any(e => e.MarketResearchBulletPointId == id);
        }

        #endregion


    }
}