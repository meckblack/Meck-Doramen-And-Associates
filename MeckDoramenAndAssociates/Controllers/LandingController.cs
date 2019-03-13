using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeckDoramenAndAssociates.Controllers
{
    public class LandingController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public LandingController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Index

        [Route("landing/index")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Index()
        {
            var userObject = _session.GetString("MDnAloggedinuser");
            var _user = JsonConvert.DeserializeObject<ApplicationUser>(userObject);

            ViewData["loggedinuserfullname"] = _user.DisplayName;
            ViewData["loggedinuseremail"] = _user.Email;

            dynamic mymodel = new ExpandoObject();
            mymodel.Logos = GetLogos();
            mymodel.Contacts = GetContacts();
            mymodel.LandingAboutUs = GetLandingAboutUs();
            mymodel.Vision = GetVision();
            mymodel.LandingSkill = GetLandingSkill();
            mymodel.HeaderImage = GetHeaderImage();
            mymodel.Brochure = GetBrochure();
            mymodel.FooterAboutUs = GetFooterAboutUs();
            
            ViewData["logochecker"] = _database.Logo.Count();
            ViewData["contactchecker"] = _database.Contacts.Count();
            ViewData["landingaboutuschecker"] = _database.LandingAboutUs.Count();
            ViewData["visionchecker"] = _database.Vision.Count();
            ViewData["landingskillchecker"] = _database.LandingSkills.Count();
            ViewData["headerimage"] = _database.HeaderImages.Count();
            ViewData["brochure"] = _database.Brochure.Count();
            ViewData["footeraboutus"] = _database.FooterAboutUs.Count();

            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
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

            return View(mymodel);
        }

        #endregion

        #region Get Logo

        private List<Logo> GetLogos()
        {
            var _logos = _database.Logo.ToList();

            return _logos;
        }

        #endregion

        #region Get Contacts

        private List<Contacts> GetContacts()
        {
            var _contacts = _database.Contacts.ToList();

            return _contacts;
        }

        #endregion

        #region Get Landing About Us

        private List<LandingAboutUs> GetLandingAboutUs()
        {
            var _landingAboutUs = _database.LandingAboutUs.ToList();
            return _landingAboutUs;
        }

        #endregion

        #region Get Vision

        private List<Vision> GetVision()
        {
            var _vision = _database.Vision.ToList();

            return _vision;
        }

        #endregion

        #region Get Landing Skill

        private List<LandingSkill> GetLandingSkill()
        {
            var _landingskill = _database.LandingSkills.ToList();

            return _landingskill;
        }

        #endregion

        #region Get Header Image

        private List<HeaderImage> GetHeaderImage()
        {
            var _headerImage = _database.HeaderImages.ToList();

            return _headerImage;
        }

        #endregion

        #region Get FooterAboutUs

        private List<FooterAboutUs> GetFooterAboutUs()
        {
            var _footerAboutUs = _database.FooterAboutUs.ToList();

            return _footerAboutUs;
        }

        #endregion

        #region Get Brochure

        private List<Brochure> GetBrochure()
        {
            var _brochure = _database.Brochure.ToList();

            return _brochure;
        }

        #endregion

    }
}