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
        [SessionExpireFilterAttribute]
        [Route("paragraph/index/{id}")]
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

            _session.SetInt32("subserviceid", Convert.ToInt32(id));

            var _paragraphs = await _database.Paragraphs.Where(paragraph => paragraph.SubServiceId == id).ToListAsync();
            return View(_paragraphs);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("paragraph/create")]
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

            var id = _session.GetInt32("subserviceid");
            ViewBag.SubServiceId = new SelectList(_database.SubServices.Where(subservice => subservice.SubServiceId == id), "SubServiceId", "Name");

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

            TempData["paragraph"] = "You have encountered and error. Kindly try adding the Paragraph again !!!";
            TempData["notificationType"] = NotificationType.Success.ToString();

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

            var _paragraph = await _database.Paragraphs.SingleOrDefaultAsync(p => p.ParagraphId == id);

            if(_paragraph == null)
            {
                return RedirectToAction("Index", "Edit");
            }

            var suberserviceid = _session.GetInt32("subserviceid");

            ViewBag.SubServiceId = new SelectList(_database.SubServices.Where(ss => ss.SubServiceId == suberserviceid), "SubServiceId", "Name");

            return PartialView("Edit", _paragraph);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id, Paragraph paragraph)
        {
            if(id != paragraph.ParagraphId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    paragraph.DateLastModified = DateTime.Now;
                    paragraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Paragraphs.Update(paragraph);
                    await _database.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParagraphExists(paragraph.ParagraphId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["paragraph"] = "You have successfully modified Meck Doramen And Associates's Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            TempData["paragraph"] = "You have encountered and error. Kindly try editing the Paragraph again !!!";
            TempData["notificationType"] = NotificationType.Success.ToString();

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

            if (role.CanManageNews == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

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
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageNews == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

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
            var subservice = await _database.SubServices.FindAsync(subserviceid);
            ViewData["subservicename"] = subservice.Name;

            return PartialView("Details", _paragraph);
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