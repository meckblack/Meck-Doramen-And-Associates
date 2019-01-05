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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class MarketResearchController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public MarketResearchController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            var _mareketResearch = await _database.MarketResearches.ToListAsync();
            return View(_mareketResearch);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("marketresearch/create")]
        public IActionResult Create()
        {
            var _marketResearch = new MarketResearch();
            return PartialView("Create", _marketResearch);
        }

        [HttpPost]
        [Route("marketresearch/create")]
        public async Task<IActionResult> Create(MarketResearch marketresearch)
        {
            if (ModelState.IsValid)
            {
                marketresearch.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketresearch.DateCreated = DateTime.Now;
                marketresearch.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketresearch.DateLastModified = DateTime.Now;

                TempData["marketresearch"] = "You have successfully added Meck Doramen And Associates's Market Research!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.MarketResearches.AddAsync(marketresearch);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            return View(marketresearch);
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Edit", _marketResearch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, MarketResearch _marketResearch)
        {
            if (id != _marketResearch.MarketResearchId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _marketResearch.DateLastModified = DateTime.Now;
                    _marketResearch.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(_marketResearch);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketResearchExists(_marketResearch.MarketResearchId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["marketresearch"] = "You have successfully modified Meck Doramen And Associates's Market Research !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View(_marketResearch);
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

            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _marketResearch);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch != null)
            {
                _database.MarketResearches.Remove(_marketResearch);
                await _database.SaveChangesAsync();

                TempData["marketresearch"] = "You have successfully deleted Meck Doramen And Associates Market Research!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "MarketResearch");
        }

        #endregion

        #region Market Research Exists

        private bool MarketResearchExists(int id)
        {
            return _database.MarketResearches.Any(e => e.MarketResearchId == id);
        }

        #endregion
    }
}