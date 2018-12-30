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
        //[SessionExpireFilterAttribute]
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
            
            ViewData["logochecker"] = _database.Logo.Count();
            ViewData["contactchecker"] = _database.Contacts.Count();
            ViewData["landingaboutuschecker"] = _database.LandingAboutUs.Count();
            ViewData["visionchecker"] = _database.Vision.Count();
            ViewData["landingskillchecker"] = _database.LandingSkills.Count();

            var roleid = _user.RoleId;

            var role = _database.Roles.Find(roleid);

            ViewData["userrole"] = role.Name;

            if (role.CanDoEverything == false)
            {
                return RedirectToAction("Index", "Error");
            }

            ViewData["candoeverything"] = await _database.Roles.SingleOrDefaultAsync(r => r.CanDoEverything == true && r.RoleId == roleid);

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
    }
}