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
    public class SkillsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHostingEnvironment _environment;

        #region Constructor

        public SkillsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IHostingEnvironment environment)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }

        #endregion

        #region Index

        [Route("skills/index")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index()
        {
            var userObject = _session.GetString("MDnAloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

            return View(await _database.Skills.ToListAsync());
        }

        #endregion

        #region Create

        [HttpGet]
        [SessionExpireFilter]
        [Route("skill/create")]
        public async Task<IActionResult> Create()
        {
            var counter = _database.Skills.Count();

            if (counter == 6)
            {
                TempData["landing"] = "Sorry the skill limit has reached. You can change it by deleting first before adding!!!";
                TempData["notificationType"] = NotificationType.Info.ToString();
                return RedirectToAction("Index", "Skills");
            }

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
        [Route("skill/create")]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Skills skills, IFormFile file)
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
                    skills.Image = filename;
                    skills.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    skills.DateCreated = DateTime.Now;
                    skills.DateLastModified = DateTime.Now;
                    skills.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    TempData["skills"] = "You have successfully added Meck Doramen And Associates's Skill !!!";
                    TempData["notificationType"] = NotificationType.Success.ToString();

                    await _database.Skills.AddAsync(skills);
                    await _database.SaveChangesAsync();
                    
                    return RedirectToAction("Index", "Skills");
                }
            }
            return View(skills);
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilter]
        [Route("skill/edit")]
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

            var _skills = await _database.Skills.SingleOrDefaultAsync(s => s.SkillsId == id);

            if (_skills == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(_skills);
        }

        [HttpPost]
        [Route("skill/edit")]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(Skills skills, IFormFile file)
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
                        skills.Image = filename;
                        skills.DateLastModified = DateTime.Now;
                        skills.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                        TempData["skills"] = "You have successfully modified Meck Doramen And Associates's Skill !!!";
                        TempData["notificationType"] = NotificationType.Success.ToString();

                        _database.Update(skills);
                        await _database.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!SkillsExists(skills.SkillsId))
                        {
                            return RedirectToAction("Index", "Error");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction("Index", "Skills");
                }
            }
            return View(skills);
        }

        #endregion

        #region View

        [HttpGet]
        [Route("skill/view")]
        public async Task <IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _skill = await _database.Skills.SingleOrDefaultAsync(s => s.SkillsId == id);
            
            if(_skill == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView(_skill);
        }

        #endregion

        #region Details

        [HttpGet]
        [Route("skill/details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _skills = await _database.Skills.SingleOrDefaultAsync(s => s.SkillsId == id);

            if (_skills == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(_skills);
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

            var _skills = await _database.Skills.SingleOrDefaultAsync(s => s.SkillsId == id);

            if (_skills == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return View("Delete", _skills);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _skills = await _database.Skills.SingleOrDefaultAsync(s => s.SkillsId == id);

            if (_skills != null)
            {
                _database.Skills.Remove(_skills);
                await _database.SaveChangesAsync();

                TempData["skills"] = "You have successfully deleted Meck Doramen And Associates Skill!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return RedirectToAction("Index", "Skills");
        }

        #endregion

        #region Skills Exists

        private bool SkillsExists(int id)
        {
            return _database.Skills.Any(e => e.SkillsId == id);
        }

        #endregion
    }
}