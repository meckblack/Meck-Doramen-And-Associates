using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Services;
using MeckDoramenAndAssociates.Data;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

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
            mymodel.Service = GetServices();
            mymodel.News = GetNews();
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
            
            foreach (FooterAboutUs footerAboutUs in mymodel.FooterAboutUs)
            {
                ViewData["footeraboutus"] = footerAboutUs.WriteUp;
            }


            return View(mymodel);
        }

        #endregion

        #region Download

        //[HttpPost]
        //public async Task<IActionResult> Download(string filename)
        //{
        //    if(filename == null)
        //    {
        //        return Content("filename not present");
        //    }

        //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles/News", filename);

        //    var memory = new MemoryStream();
        //    using(var stream = new FileStream(path, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, GetContentType(path), Path.GetFileName(path));
        //}

        public FileResult Download(string ImageName)
        {
            var path = "" + ImageName;
            return File(path, "application/force- download", Path.GetFileName(path));
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
            var _news = _database.News.OrderByDescending(x => x.NewsId).Take(3).ToList();

            return _news;
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
