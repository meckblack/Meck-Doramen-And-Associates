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
    public class AboutUsParagraphController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AboutUsParagraphController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [SessionExpireFilterAttribute]
        [Route("aboutusparagraph/index/{id}")]
        public async Task<IActionResult> Index(int id)
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

            ViewData["CanManageLandingDetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageLandingDetails == true);
            ViewData["CanManageNews"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageNews == true);
            ViewData["CanMangeUsers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanMangeUsers == true);
            ViewData["CanManageServices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageServices == true);
            ViewData["CanManageMarketResearch"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageMarketResearch == true);
            ViewData["CanManageAboutUs"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageEnquiry"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageEnquiry == true);
            ViewData["CanManagePartner"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManagePartner == true);

            #endregion

            _session.SetInt32("aboutusid", id);
            var _aboutUsParagraph = await _database.AboutUsParagraph.Where(aup => aup.AboutUsId == id).ToListAsync();
            return View(_aboutUsParagraph);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutusparagraph/create")]
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

            var aboutUsId = _session.GetInt32("aboutusid");
            ViewBag.AboutUs = new SelectList(_database.AboutUs.Where(au => au.AboutUsId == aboutUsId), "AboutUsId", "Name");

            var _aboutUsParagraph = new AboutUsParagraph();
            return PartialView("Create", _aboutUsParagraph);
        }

        [HttpPost]
        [Route("aboutusparagraph/create")]
        public async Task<IActionResult> Create(AboutUsParagraph aboutUsParagraph)
        {
            if (ModelState.IsValid)
            {
                aboutUsParagraph.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsParagraph.DateCreated = DateTime.Now;
                aboutUsParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUsParagraph.DateLastModified = DateTime.Now;

                TempData["aboutusparagraph"] = "You have successfully added Meck Doramen And Associates's About Us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.AboutUsParagraph.AddAsync(aboutUsParagraph);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            TempData["aboutusparagraph"] = "Sorry you have encountered an error. Kindly try adding the About Us Paragraph again!!!";
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var aboutUsId = _session.GetInt32("aboutusid");
            ViewBag.AboutUs = new SelectList(_database.AboutUs.Where(au => au.AboutUsId == aboutUsId), "AboutUsId", "Name");

            return PartialView("Edit", _aboutUsParagraph);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AboutUsParagraph aboutUsParagraph)
        {
            if (id != aboutUsParagraph.AboutUsParagraphId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aboutUsParagraph.DateLastModified = DateTime.Now;
                    aboutUsParagraph.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(aboutUsParagraph);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsParagraphExists(aboutUsParagraph.AboutUsParagraphId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["aboutusparagraph"] = "You have successfully modified Meck Doramen And Associates's About us Paragraph !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            TempData["aboutusparagraph"] = "Sorry you have encountered an error. Kindly try editing the About Us Paragraph again!!!";
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _aboutUsParagraph);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(s => s.AboutUsParagraphId == id);

            if (_aboutUsParagraph != null)
            {
                _database.AboutUsParagraph.Remove(_aboutUsParagraph);
                await _database.SaveChangesAsync();

                TempData["aboutusparagraph"] = "You have successfully deleted Meck Doramen And Associates About Us Paragraph!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "AboutUsParagraph");
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

            var _aboutUsParagraph = await _database.AboutUsParagraph.SingleOrDefaultAsync(a => a.AboutUsParagraphId == id);

            if(_aboutUsParagraph == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var aboutUsId = _aboutUsParagraph.AboutUsId;
            var aboutUs = await _database.AboutUs.FindAsync(aboutUsId);
            ViewData["aboutusname"] = aboutUs.Name;

            return PartialView("Details", _aboutUsParagraph);
        }

        #endregion

        #region About Us Paragraph Exists

        private bool AboutUsParagraphExists(int id)
        {
            return _database.AboutUsParagraph.Any(e => e.AboutUsParagraphId == id);
        }

        #endregion


    }
}