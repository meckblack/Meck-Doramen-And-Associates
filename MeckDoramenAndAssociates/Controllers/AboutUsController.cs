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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class AboutUsParagraphBullet
    {
        public AboutUs aboutUs { get; set; }
        public AboutUsParagraph aboutUsParagraph { get; set; }
        public List<AboutUsBulletPoint> aboutUsBulletPoints { get; set; }
    }

    public class AboutUsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AboutUsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            ViewData["CanManageLandingDetails"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageNews"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanMangeUsers"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageServices"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageMarketResearch"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageAboutUs"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);
            ViewData["CanManageEnquiry"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanManageAboutUs == true);

            #endregion

            var _aboutUs = await _database.AboutUs.ToListAsync();
            return View(_aboutUs);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("aboutus/create")]
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

            var _aboutUs = new AboutUs();
            return PartialView("Create", _aboutUs);
        }

        [HttpPost]
        [Route("aboutus/create")]
        public async Task<IActionResult> Create(AboutUs aboutUs)
        {
            if (ModelState.IsValid)
            {
                aboutUs.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUs.DateCreated = DateTime.Now;
                aboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                aboutUs.DateLastModified = DateTime.Now;

                TempData["aboutus"] = "You have successfully added Meck Doramen And Associates's About Us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.AboutUs.AddAsync(aboutUs);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            return View(aboutUs);
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

            var _aboutUs= await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }
            
            return PartialView("Edit", _aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, AboutUs aboutUs)
        {
            if (id != aboutUs.AboutUsId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    aboutUs.DateLastModified = DateTime.Now;
                    aboutUs.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(aboutUs);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutUsExists(aboutUs.AboutUsId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["abooutus"] = "You have successfully modified Meck Doramen And Associates's About us !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            
            return View(aboutUs);
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

            var _aboutUs = await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _aboutUs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _aboutUs = await _database.AboutUs.SingleOrDefaultAsync(s => s.AboutUsId == id);

            if (_aboutUs != null)
            {
                _database.AboutUs.Remove(_aboutUs);
                await _database.SaveChangesAsync();

                TempData["aboutus"] = "You have successfully deleted Meck Doramen And Associates About Us!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "AboutUs");
        }

        #endregion

        #region ReadMore

        [HttpGet]
        [Route("aboutus/readmore")]
        public IActionResult ReadMore()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.AboutUs = GetAboutUs();
            mymodel.AboutUsParagraph = GetAboutUsParagraphs();

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

        #region About Us Exists

        private bool AboutUsExists(int id)
        {
            return _database.AboutUs.Any(e => e.AboutUsId == id);
        }

        #endregion

        #region Get Contacts

        private List<Contacts> GetContacts()
        {
            var _contacts = _database.Contacts.ToList();

            return _contacts;
        }

        #endregion

        #region Get AboutUs Paragraph

        private List<AboutUsParagraphBullet> GetAboutUsParagraphs()
        {
            var _aboutUsParagraph = _database.AboutUsParagraph.ToList();
            var aboutUsParagraphStore = new List<AboutUsParagraphBullet>();
            foreach(AboutUsParagraph aboutUsParagraph in _aboutUsParagraph)
            {
                var aboutUsBulletPoints = _database.AboutUsBulletPoint.Where(aub => aub.AboutUsParagraphId == aboutUsParagraph.AboutUsParagraphId).ToList();
                var _object = new AboutUsParagraphBullet();
                _object.aboutUsParagraph = aboutUsParagraph;
                _object.aboutUsBulletPoints = aboutUsBulletPoints;
                aboutUsParagraphStore.Add(_object);
            }
            return aboutUsParagraphStore;
        }

        #endregion

        #region Get Logo

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logo.ToList();

            return _logos;
        }

        #endregion

        #region Get About US

        private List<AboutUs> GetAboutUs()
        {
            var _aboutUs = _database.AboutUs.ToList();

            return _aboutUs;
        }

        #endregion
    }
}