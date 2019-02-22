﻿using System;
using System.Collections.Generic;
using System.Dynamic;
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

        #region Constructor

        public ServiceController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            var _services = await _database.Services.ToListAsync();
            return View(_services);
        }

        #endregion

        #region ViewAllServices

        [HttpGet]
        [Route("service/viewallservices")]
        public async Task<IActionResult> ViewAllServices()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.LandingSkill = GetLandingSkills();

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["logo"] = logo.Image;
            }

            foreach (Contacts contacts in mymodel.Contacts)
            {
                ViewData["address"] = contacts.Address;
                ViewData["email"] = contacts.Email;
                ViewData["number"] = contacts.Number;
                ViewData["openweekdays"] = contacts.OpenWeekdays;
                ViewData["weekdaytimeopen"] = contacts.WeekdaysOpenTime.TimeOfDay;
                ViewData["weekdaytimeclose"] = contacts.WeekdaysCloseTime.TimeOfDay;
                ViewData["openweekends"] = contacts.OpenWeekends;
                ViewData["weekendtimeopen"] = contacts.WeekendsOpenTime.TimeOfDay;
                ViewData["weekendtimeclose"] = contacts.WeekendsCloseTIme.TimeOfDay;
            }

            foreach (LandingSkill skill in mymodel.LandingSkill)
            {
                ViewData["Landingheader"] = skill.Header;
                ViewData["Landingbody"] = skill.Body;
            }
            
            var allservices = await _database.Services.ToListAsync();
            return View(allservices);
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

            return PartialView("Create");
        }

        [HttpPost]
        [Route("service/create")]
        [SessionExpireFilter]
        [ValidateAntiForgeryToken]
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

                return RedirectToAction("Index", "Service");
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

            return PartialView("Edit", _service);
        }

        [HttpPost]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction("Index", "Service");
            }

            TempData["service"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index", "Service");
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
        
        #region Explanation

        [HttpGet]
        [Route("service/explanation")]
        public async Task<IActionResult> Explanation(int? id)
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

            ViewData["name"] = _service.Name;
            ViewData["body"] = _service.Explanation;

            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.HeaderImage = GetHeaderImage();
            mymodel.FooterImage = GetFooterImage();
            mymodel.SubService = GetSubServices(id);

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["logo"] = logo.Image;
            }

            foreach (Contacts contacts in mymodel.Contacts)
            {
                ViewData["address"] = contacts.Address;
                ViewData["email"] = contacts.Email;
                ViewData["number"] = contacts.Number;
                ViewData["openweekdays"] = contacts.OpenWeekdays;
                ViewData["weekdaytimeopen"] = contacts.WeekdaysOpenTime.TimeOfDay;
                ViewData["weekdaytimeclose"] = contacts.WeekdaysCloseTime.TimeOfDay;
                ViewData["openweekends"] = contacts.OpenWeekends;
                ViewData["weekendtimeopen"] = contacts.WeekendsOpenTime.TimeOfDay;
                ViewData["weekendtimeclose"] = contacts.WeekendsCloseTIme.TimeOfDay;
            }

            foreach (HeaderImage headerImage in mymodel.HeaderImage)
            {
                ViewData["headerimage"] = headerImage.Image;
            }

            foreach (FooterImage footerImage in mymodel.FooterImage)
            {
                ViewData["footerimage"] = footerImage.Image;
            }

            return View(mymodel);
        }

        #endregion

        #region Service Exists

        private bool ServiceExists(int id)
        {
            return _database.Services.Any(e => e.ServiceId == id);
        }

        #endregion

        #region Get Footer Image

        private List<FooterImage> GetFooterImage()
        {
            var _footerImage = _database.FooterImages.ToList();

            return _footerImage;
        }

        #endregion

        #region Get Contacts

        private List<Contacts> GetContacts()
        {
            var _contacts = _database.Contacts.ToList();

            return _contacts;
        }

        #endregion

        #region Get Logo

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logo.ToList();

            return _logos;
        }

        #endregion

        #region Get Header Image

        private List<HeaderImage> GetHeaderImage()
        {
            var _headerImage = _database.HeaderImages.ToList();

            return _headerImage;
        }

        #endregion

        #region Get Sub Service

        private List<SubService> GetSubServices(int? id)
        {
            var _subService = _database.SubServices.Where(s => s.ServiceId == id).ToList();

            return _subService;
        }

        #endregion

        #region Get Landing Skill

        private List<LandingSkill> GetLandingSkills()
        {
            var _landingSkill = _database.LandingSkills.ToList();
            return _landingSkill;
        }

        #endregion


    }
}