using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class ParagraphBullet
    {
        public Paragraph paragraph { get; set; }
        public List<BulletPoint> bullets { get; set; }
    }

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
        [SessionExpireFilterAttribute]
        [Route("subservice/index/{id}")]
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

            _session.SetInt32("serviceid", Convert.ToInt32(id));
            var _subServices = await _database.SubServices.Where(subservice => subservice.ServiceId == id).ToListAsync();
            return View(_subServices);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("subservice/create")]
        public async Task<IActionResult> Create()
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

            var serviceid = _session.GetInt32("serviceid");
            ViewBag.ServiceId = new SelectList(_database.Services.Where(subservice => subservice.ServiceId == serviceid), "ServiceId", "Name");

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

            var serviceid = _session.GetInt32("serviceid");
            ViewBag.ServiceId = new SelectList(_database.Services.Where(subservice => subservice.ServiceId == serviceid), "ServiceId", "Name", _subService.ServiceId);

            TempData["subservice"] = "Sorry an error occured, Try adding the subservice again !!!";
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

            var _subService = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subService == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var serviceid = _session.GetInt32("serviceid");
            ViewBag.ServiceId = new SelectList(_database.Services.Where(subservice => subservice.ServiceId == serviceid), "ServiceId", "Name");

            return PartialView("Edit", _subService);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, SubService _subService)
        {
            if(id != _subService.SubServiceId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _subService.DateLastModified = DateTime.Now;
                    _subService.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                    
                    _database.Update(_subService);
                    await _database.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubServiceExists(_subService.SubServiceId))
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

            var serviceid = _session.GetInt32("serviceid");
            ViewBag.ServiceId = new SelectList(_database.Services.Where(subservice => subservice.ServiceId == serviceid), "ServiceId", "Name", _subService.ServiceId);

            return View(_subService);
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

        #region Explanation

        [HttpGet]
        [Route("subservice/explanation/{id}")]
        public async Task<IActionResult> Explanation(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _subservice = await _database.SubServices.SingleOrDefaultAsync(s => s.SubServiceId == id);

            if (_subservice == null)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["name"] = _subservice.Name;

            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.Paragraph = GetParagraphs(id);
            
            //mymodel.BulletPoint = paragraphStore;

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
            
            return View(mymodel);
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
        
        #region Get Paragraph

        private List<ParagraphBullet> GetParagraphs(int? id)
        {
            var _paragraph = _database.Paragraphs.Where(p => p.SubServiceId == id).ToList();
            var paragraphStore = new List<ParagraphBullet>();
            foreach (Paragraph paragraph in _paragraph)
            {
                var bulletpoints = _database.BulletPoints.Where(b => b.ParagraphId == paragraph.ParagraphId).ToList();
                var _object = new ParagraphBullet();
                _object.paragraph = paragraph;
                _object.bullets = bulletpoints;
                paragraphStore.Add(_object);
            }
            return paragraphStore;
        }

        #endregion

        #region Get Bullet Points

        private List<BulletPoint> GetBulletPoints(int? id)
        {
            var _bulletPoint = _database.BulletPoints.Where(b => b.BulletPointId == id).ToList();
            //var _bulletPoint = _database.BulletPoints.Where(b => b.BulletPointId == id).Include(b => b.Paragraph).ToList();
            return _bulletPoint;
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