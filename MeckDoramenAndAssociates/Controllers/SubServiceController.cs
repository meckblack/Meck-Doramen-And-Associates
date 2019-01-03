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
    public class SubServiceController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public SubServiceController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [Route("subservice/index")]
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

            var _subServices = await _database.SubServices.ToListAsync();
            return View(_subServices);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("subservice/create")]
        public IActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(_database.Services, "ServiceId", "Name");

            var _subService = new SubService();
            return PartialView("Create", _subService);
        }

        [HttpPost]
        [Route("subservice/create")]
        public async Task<IActionResult> Create(SubService _subService)
        {
            if (ModelState.IsValid)
            {
                _subService.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                _subService.DateCreated = DateTime.Now;
                _subService.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                _subService.DateLastModified = DateTime.Now;

                TempData["subservice"] = "You have successfully added Meck Doramen And Associates's Subservice !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.SubServices.AddAsync(_subService);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            ViewBag.ServiceId = new SelectList(_database.Services, "ServiceId", "Name", _subService.ServiceId);
            return View(_subService);
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

            var _subService = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subService == null)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewBag.ServiceId = new SelectList(_database.Services, "ServiceId", "Name");
            return PartialView("Edit", _subService);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, SubService subService)
        {
            if(id != subService.SubServiceId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subService.DateLastModified = DateTime.Now;
                    subService.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    
                    _database.Update(subService);
                    await _database.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubServiceExists(subService.SubServiceId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["subservice"] = "You have successfully modified Meck Doramen And Associates's Sub Service !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });   
            }

            ViewBag.ServiceId = new SelectList(_database.Services, "ServiceId", "Name", subService.ServiceId);
            return View(subService);
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

            var _subService = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subService == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _subService);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _subService = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subService != null)
            {
                _database.SubServices.Remove(_subService);
                await _database.SaveChangesAsync();

                TempData["subservice"] = "You have successfully deleted Meck Doramen And Associates Sub Service!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "SubService");
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

            var _subService = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subService == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var serviceid = _subService.ServiceId;
            var service = await _database.Services.FindAsync(serviceid);
            ViewData["servicename"] = service.Name;

            return PartialView("Details", _subService);
        }

        #endregion

        #region Sub Service Exists

        private bool SubServiceExists(int id)
        {
            return _database.SubServices.Any(e => e.SubServiceId == id);
        }

        #endregion
    }
}