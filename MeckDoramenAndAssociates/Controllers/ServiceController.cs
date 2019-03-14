using System;
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
                ViewData["number2"] = contacts.Number2;
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
        public async Task<IActionResult> Create()
        {
            #region Checker

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);
            
            if (role.CanManageServices == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            var service = new Service();
            return PartialView("Create", service);
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

                TempData["service"] = "You have successfully added a new Service to Meck Doramen And Associates's Services !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.Services.AddAsync(service);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }

            TempData["service"] = "Sorry an error occured. Try Creating the Service again !!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index");
        }

        #endregion

        #region Edit

        [HttpGet]
        [SessionExpireFilter]
        public async Task<IActionResult> Edit(int? id)
        {
            #region Checker

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

                    TempData["service"] = "You have successfully modified " + service.Name + " Meck Doramen And Associates's Service !!!";
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

                return Json(new { success = true });
            }

            TempData["service"] = "So please try again an error ecored!!!";
            TempData["notificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Index", "Service");
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            #region Checker

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

            var _service = await _database.Services.SingleOrDefaultAsync(s => s.ServiceId == id);

            if (_service == null)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return PartialView("Details", _service);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            #region Checker

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
            mymodel.SubService = GetSubServices(id);
            mymodel.FooterAboutUs = GetFooterAboutUs();
            mymodel.Partners = GetPartners();

            foreach (Logo logo in mymodel.Logos)
            {
                ViewData["logo"] = logo.Image;
            }

            foreach (Contacts contacts in mymodel.Contacts)
            {
                ViewData["address"] = contacts.Address;
                ViewData["email"] = contacts.Email;
                ViewData["number"] = contacts.Number;
                ViewData["number2"] = contacts.Number2;
            }

            foreach (FooterAboutUs footerAboutUs in mymodel.FooterAboutUs)
            {
                ViewData["footeraboutus"] = footerAboutUs.WriteUp;
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

        #region Get Footer About US

        private List<FooterAboutUs> GetFooterAboutUs()
        {
            var _footerAboutUs = _database.FooterAboutUs.ToList();

            return _footerAboutUs;
        }

        #endregion

        #region Get Partners

        private List<Partner> GetPartners()
        {
            var _partners = _database.Partners.ToList();

            return _partners;
        }

        #endregion



    }
}