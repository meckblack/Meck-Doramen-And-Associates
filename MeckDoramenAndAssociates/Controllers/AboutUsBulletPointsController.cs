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
        public async Task<IActionResult> Index()
        {
            var userObject = _session.GetString("MDnAloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;
            var role = await _database.Roles.FindAsync(roleid);
            ViewData["userrole"] = role.Name;

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == _user.RoleId);

            var _aboutUsBulletPoint = await _database.AboutUs.ToListAsync();
            return View(_aboutUsBulletPoint);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutusbulletpoint/create")]
        public IActionResult Create()
        {
            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph, "AboutUsParagraphId", "Body");

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

            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph, "AboutUsParagraphId", "Body", aboutUsBulletPoint.AboutUsParagraphId);
            return View(aboutUsBulletPoint);
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

            var _aboutUsBulletPoint = await _database.AboutUsBulletPoint.SingleOrDefaultAsync(s => s.AboutUsBulletPointId == id);

            if (_aboutUsBulletPoint == null)
            {
                return RedirectToAction("Index", "Error");
            }
            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph, "AboutUsParagraphId", "Body");

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
                TempData["abooutusbulletpoint"] = "You have successfully modified Meck Doramen And Associates's About us Bullet Point !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            ViewBag.AboutUsParagraph = new SelectList(_database.AboutUsParagraph, "AboutUsParagraphId", "Body", aboutUsBulletPoint.AboutUsParagraphId);
            return View(aboutUsBulletPoint);
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

        #region About Us Bullet Point Exists

        private bool AboutUsBulletPointExists(int id)
        {
            return _database.AboutUsBulletPoint.Any(e => e.AboutUsBulletPointId == id);
        }

        #endregion
    }
}