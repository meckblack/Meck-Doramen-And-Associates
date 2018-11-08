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

namespace MeckDoramenAndAssociates.Controllers
{
    public class ContactsController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public ContactsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        public IActionResult Index()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();



            foreach (Contacts contact in mymodel.Contacts)
            {

            }

            foreach (Logo logo in mymodel.Logos)
            {
                _session.SetString("imageoflogo", logo.Image);

                ViewData["imageoflogo"] = logo.Image;
            }

            var customerObject = _session.GetString("imouloggedincustomer");
            if (customerObject == null)
            {
                return View();
            }

            return View();
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            var contact = new Contacts();
            return PartialView("Create", contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Create(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                contact.DateCreated = DateTime.Now;
                contact.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                contact.DateLastModified = DateTime.Now;
                contact.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
             
                await _database.Contacts.AddAsync(contact);
                await _database.SaveChangesAsync();

                TempData["landing"] = "You have successfully added the contact details";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion
        
        #region Delete

        // GET: Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var contact = await _database.Contacts
                .FirstOrDefaultAsync(m => m.ContactsId == id);
            if (contact == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", contact);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var _contact = await _database.Contacts.FindAsync(id);
            if (_contact != null)
            {
                _database.Contacts.Remove(_contact);
                await _database.SaveChangesAsync();
                TempData["landing"] = "You have successfully deleted the contact details";
                TempData["notificationType"] = NotificationType.Success.ToString();
                return Json(new { success = true });
            }
            return View("Index");
        }

        #endregion

        #region View

        // GET: Contact/View/5
        public async Task<IActionResult> View(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var contact = await _database.Contacts
                .FirstOrDefaultAsync(m => m.ContactsId == id);
            if (contact == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("View", contact);
        }

        #endregion

        #region Exists

        private bool ContactExists(int id)
        {
            return _database.Contacts.Any(e => e.ContactsId == id);
        }

        #endregion

        #region Get Logos

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logo.ToList();

            return _logos;
        }

        #endregion

        #region Get Contacts

        private List<Contacts> GetContacts()
        {
            var _contact = _database.Contacts.ToList();
            return _contact;
        }

        #endregion
    }
}