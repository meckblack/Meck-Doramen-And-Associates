using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Models.Enums;
using MeckDoramenAndAssociates.Services;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public ServiceController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
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

            var _services = await _database.Services.ToListAsync();
            return View(_services);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("service/create")]
        [SessionExpireFilter]
        public async Task<IActionResult> Create()
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return View();
        }

        [HttpPost]
        [Route("service/create")]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Services");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    service.Image = filename;
                    service.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    service.DateCreated = DateTime.Now;
                    service.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    service.DateLastModified = DateTime.Now;

                    TempData["service"] = "You have successfully added Meck Doramen And Associates's Service !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Services.AddAsync(service);
                    await _database.SaveChangesAsync();

                    return RedirectToAction("Index", "Service");
                }
            }
            
            return View(service);
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            var userid = _session.GetInt32("MDnAloggedinuserid");
            var _user = await _database.ApplicationUsers.FindAsync(userid);
            ViewData["loggedinuserfullname"] = _user.DisplayName;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if (_service == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Edit", _service);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(Service service, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("null_img", "File not selected");
            }
            else
            {
                var fileinfo = new FileInfo(file.FileName);
                var filename = DateTime.Now.ToFileTime() + fileinfo.Extension;
                var uploads = Path.Combine(_environment.WebRootPath, "UploadedFiles\\Skills");
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        service.Image = filename;
                        service.DateLastModified = DateTime.Now;
                        service.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                        TempData["services"] = "You have successfully modified Meck Doramen And Associates's Service !!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Update(service);
                        await _database.SaveChangesAsync();
                        
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
                    return RedirectToAction("Index", "Services");
                }
            }
            return View(service);
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