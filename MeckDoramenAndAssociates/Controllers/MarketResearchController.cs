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
    public class MarketResearchController : Controller
    {
        public class MarketResearchParagraphBullet
        {
            public MarketResearch marketResearch { get; set; }
            public MarketResearchParagraph marketResearchParagraph { get; set; }
            public List<MarketResearchBulletPoint> MarketResearchBullets { get; set; }
        }

        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public MarketResearchController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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

            if (role.CanManageMarketResearch == false)
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

            var _mareketResearch = await _database.MarketResearches.ToListAsync();
            return View(_mareketResearch);
        }

        #endregion

        #region Create

        [HttpGet]
        [Route("marketresearch/create")]
        public async Task<IActionResult> Create()
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            var _marketResearch = new MarketResearch();
            return PartialView("Create", _marketResearch);
        }

        [HttpPost]
        [Route("marketresearch/create")]
        public async Task<IActionResult> Create(MarketResearch marketresearch)
        {
            if (ModelState.IsValid)
            {
                marketresearch.CreatedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketresearch.DateCreated = DateTime.Now;
                marketresearch.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));
                marketresearch.DateLastModified = DateTime.Now;

                TempData["marketresearch"] = "You have successfully added Meck Doramen And Associates's Market Research!!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                await _database.MarketResearches.AddAsync(marketresearch);
                await _database.SaveChangesAsync();

                return Json(new { success = true });
            }
            return View(marketresearch);
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

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Edit", _marketResearch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, MarketResearch _marketResearch)
        {
            if (id != _marketResearch.MarketResearchId)
            {
                return RedirectToAction("Index", "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _marketResearch.DateLastModified = DateTime.Now;
                    _marketResearch.LastModifiedBy = Convert.ToInt32(_session.GetInt32("MDnAloggedinuserid"));

                    _database.Update(_marketResearch);
                    await _database.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarketResearchExists(_marketResearch.MarketResearchId))
                    {
                        return RedirectToAction("Index", "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["marketresearch"] = "You have successfully modified Meck Doramen And Associates's Market Research !!!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return View(_marketResearch);
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

            if (role.CanManageMarketResearch == false)
            {
                TempData["error"] = "Sorry you are not authorized to access this page";
                return RedirectToAction("Index", "Error");
            }

            #endregion

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch == null)
            {
                return RedirectToAction("Index", "Error");
            }

            return PartialView("Delete", _marketResearch);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var _marketResearch = await _database.MarketResearches.SingleOrDefaultAsync(s => s.MarketResearchId == id);

            if (_marketResearch != null)
            {
                _database.MarketResearches.Remove(_marketResearch);
                await _database.SaveChangesAsync();

                TempData["marketresearch"] = "You have successfully deleted Meck Doramen And Associates Market Research!";
                TempData["notificationType"] = NotificationType.Success.ToString();

                return Json(new { success = true });
            }

            return RedirectToAction("Index", "MarketResearch");
        }

        #endregion

        #region Read More

        [HttpGet]
        [Route("marketresearch/readmore")]
        public IActionResult ReadMore()
        {
            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.MarketResearchParagraph = GetMarketResearchParagraph();
            mymodel.MarketResearch = GetMarketResearch();
            mymodel.Partners = GetPartners();
            mymodel.FooterAboutUs = GetFooterAboutUs();

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

        #region Get MarketResearch

        private List<MarketResearch> GetMarketResearch()
        {
            var _marketResearches = _database.MarketResearches.ToList();

            return _marketResearches;
        }

        #endregion

        #region Get Market Research Paragraph

        private List<MarketResearchParagraphBullet> GetMarketResearchParagraph()
        {
            var _markeResearchParagraph = _database.MarketResearchParagraphs.ToList();
            var marketResearchParagraphStore = new List<MarketResearchParagraphBullet>();
            
            foreach (MarketResearchParagraph marketResearchParagraph in _markeResearchParagraph)
            {
                var marketResearchBulletPoints = _database.MarketResearchBulletPoints.Where(mbp => mbp.MarketResearchParagraphId == marketResearchParagraph.MarketResearchParagraphId)
                                                                                    .ToList();
                var _object = new MarketResearchParagraphBullet();
                _object.marketResearchParagraph = marketResearchParagraph;
                _object.MarketResearchBullets = marketResearchBulletPoints;
                marketResearchParagraphStore.Add(_object);
            }
            return marketResearchParagraphStore;
        }

        #endregion

        #region Market Research Exists

        private bool MarketResearchExists(int id)
        {
            return _database.MarketResearches.Any(e => e.MarketResearchId == id);
        }

        #endregion

        #region Get Partners

        private List<Partner> GetPartners()
        {
            var _partners = _database.Partners.ToList();

            return _partners;
        }

        #endregion

        #region Get Footer About US

        private List<FooterAboutUs> GetFooterAboutUs()
        {
            var _footerAboutUs = _database.FooterAboutUs.ToList();

            return _footerAboutUs;
        }

        #endregion
    }
}