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
    public class EnquiryController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public EnquiryController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            if (role.CanManageEnquiry == false)
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

            #endregion

            var _enquiry = await _database.Enquiry.ToListAsync();
            return View(_enquiry);
        }

        #endregion

        #region Make Enquiry

        [HttpGet]
        [Route("enquiry/makeenquiry")]
        public IActionResult MakeEnquiry()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

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

            return View();
        }

        [HttpPost]
        [Route("enquiry/makeenquiry")]
        public async Task<IActionResult> MakeEnquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                var _enquiry = new Enquiry()
                {
                    FullName = enquiry.FullName,
                    Email = enquiry.Email,
                    Message = enquiry.Message,
                    PhoneNumber = enquiry.PhoneNumber
                };

                await _database.Enquiry.AddAsync(_enquiry);
                await _database.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();

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

            return View(enquiry);
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

            if (role.CanManageEnquiry == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(eq => eq.EnquiryId == id);

            if (_enquiry == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Details", _enquiry);
        }

        #endregion

        #region Delete

        [HttpGet]
        [Route("enquiry/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageEnquiry == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(s => s.EnquiryId == id);

            if (_enquiry == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _enquiry);
        }

        [HttpPost]
        [Route("enquiry/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var _enquiry = await _database.Enquiry.SingleOrDefaultAsync(s => s.EnquiryId == id);

            if (_enquiry != null)
            {
                _database.Enquiry.Remove(_enquiry);
                await _database.SaveChangesAsync();

                TempData["enquiry"] = "You have successfully deleted Meck Doramen And Associates Enquiry!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "Enquiry");
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
    }
}