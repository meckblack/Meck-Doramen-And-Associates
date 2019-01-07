using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Data;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

namespace MeckDoramenAndAssociates.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public HomeController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
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
            mymodel.LandingAboutUs = GetLandingAboutUs();
            mymodel.Vision = GetVision();
            mymodel.LandingSkill = GetLandingSkill();
            mymodel.HeaderImage = GetHeaderImage();
            mymodel.FooterImage = GetFooterImage();
            mymodel.Service = GetServices();
            mymodel.News = GetNews();

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

            foreach (LandingAboutUs landingAboutUs in mymodel.LandingAboutUs)
            {
                ViewData["header"] = landingAboutUs.Header;
                ViewData["body"] = landingAboutUs.Body;
            }

            foreach (Vision vision in mymodel.Vision)
            {
                ViewData["visionone"] = vision.VisionOne;
                ViewData["visiononerating"] = vision.VisionOneRating;

                ViewData["visiontwo"] = vision.VisionTwo;
                ViewData["visiontworating"] = vision.VisionTwoRating;

                ViewData["visionthree"] = vision.VisionThree;
                ViewData["visionthreerating"] = vision.VisionThreeRating;

                ViewData["visionfour"] = vision.VisionFour;
                ViewData["visionfourrating"] = vision.VisionFourRating;
            }

            foreach (LandingSkill landingSkill in mymodel.LandingSkill)
            {
                ViewData["skillheader"] = landingSkill.Header;
                ViewData["skillbody"] = landingSkill.Body;
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

        #region Get Footer Image

        private List<FooterImage> GetFooterImage()
        {
            var _footerImage = _database.FooterImages.ToList();

            return _footerImage;
        }

        #endregion

        #region Get Services

        private List<Service> GetServices()
        {
            var _services = _database.Services.ToList();

            return _services;
        }

        #endregion

        #region Get News

        private List<News> GetNews()
        {
            var _news = _database.News.ToList();

            return _news;
        }

        #endregion

    }
}
