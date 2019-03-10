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
    public class MarketResearchParagraphController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public MarketResearchParagraphController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("marketresearch/index/{id}")]
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

            _session.SetInt32("marketresearchid", Convert.ToInt32(id));

            var _marketResearchParagraph = await _database.MarketResearchParagraphs.Where(mrp => mrp.MarketResearchId == id).ToListAsync();
            return View(_marketResearchParagraph);
        }

        #endregion

        #region Create
        
        [HttpGet]
        [Route("marketreseachparagraph/create")]
        public async Task<IActionResult> Create()
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

            var marketresearchid = _session.GetInt32("marketresearchid");
            ViewBag.MarketResearch = new SelectList(_database.MarketResearches.Where(mrp => mrp.MarketResearchId == marketresearchid), 
                                                    "MarketResearchId", "Title");

            var _marketResearchParagraph = new MarketResearchParagraph();
            return PartialView("Create", _marketResearchParagraph);
        }

        [HttpPost]
        [Route("marketreseachparagraph/create")]
        public async Task<IActionResult> Create(MarketResearchParagraph marketResearchParagraph)
        {
            if (ModelState.IsValid)
            {
                marketResearchParagraph.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketResearchParagraph.DateCreated = DateTime.Now;
                marketResearchParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketResearchParagraph.DateLastModified = DateTime.Now;

                TempData["marketresearchparagraph"] = "You have successfully added Meck Doramen And Associates's Market Research !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.MarketResearchParagraphs.AddAsync(marketResearchParagraph);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            TempData["marketresearchparagraph"] = "Sorry you encountered an error. Kindly try adding the Market Research Paragraph again. !!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        [Route("marketresearchparagraph/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
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

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearchParagraph = await _database.MarketResearchParagraphs.SingleOrDefaultAsync(s => s.MarketResearchParagraphId == id);

            if (_marketResearchParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var marketresearchid = _session.GetInt32("marketresearchid");
            ViewBag.MarketResearch = new SelectList(_database.MarketResearches.Where(mrp => mrp.MarketResearchId == marketresearchid), 
                                                    "MarketResearchId", "Title");

            return PartialView("Edit", _marketResearchParagraph);
        }

        [Route("marketresearchparagraph/edit/{id}")]
        [HttpPost]
        public async Task<IActionResult> Edit(int? id, MarketResearchParagraph marketResearchParagraph)
        {
            if (id != marketResearchParagraph.MarketResearchParagraphId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    marketResearchParagraph.DateLastModified = DateTime.Now;
                    marketResearchParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(marketResearchParagraph);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketResearchParagraphExists(marketResearchParagraph.MarketResearchParagraphId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["marketresearchparagraph"] = "You have successfully modified Meck Doramen And Associates's Market Research Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            TempData["marketresearchparagraph"] = "Sorry you encountered an error. Kindly try editing the Market Research Paragraph again. !!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
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

            var _marketResearchParagraph = await _database.MarketResearchParagraphs.SingleOrDefaultAsync(s => s.MarketResearchParagraphId == id);

            if (_marketResearchParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _marketResearchParagraph);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _marketResearchParagraph = await _database.MarketResearchParagraphs.SingleOrDefaultAsync(s => s.MarketResearchParagraphId == id);

            if (_marketResearchParagraph != null)
            {
                _database.MarketResearchParagraphs.Remove(_marketResearchParagraph);
                await _database.SaveChangesAsync();

                TempData["marketresearchparagraph"] = "You have successfully deleted Meck Doramen And Associates Market Research Paragraph!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "MarketResearchParagraph");
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

            var _marketResearchParagraph = await _database.MarketResearchParagraphs.SingleOrDefaultAsync(a => a.MarketResearchParagraphId == id);

            if (_marketResearchParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var marketResearchId = _marketResearchParagraph.MarketResearchId;
            var marketResearch = await _database.AboutUs.FindAsync(marketResearchId);
            ViewData["marketresearchname"] = marketResearch.Name;

            return PartialView("Details", _marketResearchParagraph);
        }

        #endregion

        #region Market Research Paragraph Exists

        private bool MarketResearchParagraphExists(int id)
        {
            return _database.MarketResearchParagraphs.Any(e => e.MarketResearchParagraphId == id);
        }

        #endregion
    }
}