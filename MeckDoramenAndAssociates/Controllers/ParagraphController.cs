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
    public class ParagraphController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ParagraphController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [Route("paragraph/index")]
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

            var _paragraphs = await _database.Paragraphs.ToListAsync();
            return View(_paragraphs);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("paragraph/create")]
        public IActionResult Create()
        {
            ViewBag.SubServiceId = new SelectList(_database.SubServices, "SubServiceId", "Name");

            var paragraph = new Paragraph();
            return PartialView("Create");
        }

        [HttpPost]
        [Route("paragraph/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create(Paragraph paragraph)
        {
            if (ModelState.IsValid)
            {
                paragraph.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                paragraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                paragraph.DateCreated = DateTime.Now;
                paragraph.DateLastModified = DateTime.Now;

                await _database.Paragraphs.AddAsync(paragraph);
                await _database.SaveChangesAsync();

                TempData["paragraph"] = "You have successfully added a Meck Doramen And Associates's Sub Service Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View(paragraph);
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

            var _paragraph = await _database.Paragraphs.SingleOrDefaultAsync(s => s.ParagraphId == id);

            if (_paragraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _paragraph);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _paragraph = await _database.Paragraphs.SingleOrDefaultAsync(s => s.ParagraphId == id);

            if (_paragraph != null)
            {
                _database.Paragraphs.Remove(_paragraph);
                await _database.SaveChangesAsync();

                TempData["paragraph"] = "You have successfully deleted Meck Doramen And Associates Sub Service Paragraph!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Paragraph");
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

            var _paragraph = await _database.Paragraphs.SingleOrDefaultAsync(s => s.ParagraphId == id);

            if (_paragraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var subserviceid = _paragraph.SubServiceId;
            var subservice = await _database.Services.FindAsync(subserviceid);
            ViewData["subservicename"] = subservice.Name;

            return PartialView("Details", subservice);
        }

        #endregion

        #region Paragraph Exists

        private bool ParagraphExists(int id)
        {
            return _database.Paragraphs.Any(e => e.ParagraphId == id);
        }

        #endregion
    }
}