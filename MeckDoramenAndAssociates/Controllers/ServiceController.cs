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
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ServiceController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [HttpGet]
        [Route("service/index")]
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

            var _services = await _database.Services.ToListAsync();
            return View(_services);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("service/create")]
        public IActionResult Create()
        {
            var _service = new Service();
            return PartialView("Create", _service);
        }

        [HttpPost]
        [Route("service/create")]
        public async Task<IActionResult> Create(Service service)
        {
            if (ModelState.IsValid)
            {
                service.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                service.DateCreated = DateTime.Now;
                service.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                service.DateLastModified = DateTime.Now;

                TempData["service"] = "You have successfully added Meck Doramen And Associates's Service !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.Services.AddAsync(service);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            return RedirectToAction("Index", "Service");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if (_service == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Edit", _service);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Service service )
        {
            try
            {
                service.DateLastModified = DateTime.Now;
                service.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                TempData["services"] = "You have successfully modified Meck Doramen And Associates's Service !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                _database.Update(service);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(service.ServiceId))
                {
                    return RedirectToAction("Index", "Error");
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if (_service == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _service);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if(_service != null)
            {
                _database.Services.Remove(_service);
                await _database.SaveChangesAsync();

                TempData["serivce"] = "You have successfully deleted Meck Doramen And Associates Service!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Service");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if(_service == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Details", _service);
        }

        #endregion
        
        #region Service Exists

        private bool ServiceExists(int id)
        {
            return _database.Services.Any(e => e.ServiceId == id);
        }

        #endregion

    }
}