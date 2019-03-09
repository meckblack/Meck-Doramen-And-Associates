using MeckDoramenAndAssociates.Data;
using MeckDoramenAndAssociates.Models;
using MeckDoramenAndAssociates.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MeckDoramenAndAssociates.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _database;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        #region Constructor

        public AdminController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _database = context;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Dashboard

        [HttpGet]
        [Route("admin/dashboard")]
        [SessionExpireFilterAttribute]
        public async Task<IActionResult> Dashboard()
        {
            #region Checker

            //Checks if user is autorized to view this page

            var roleid = _session.GetInt32("MDnAloggedinuserroleid");
            var role = await _database.Roles.FindAsync(roleid);

            if (role.CanManageLandingDetails == false)
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
            
            return View();
        }

        #endregion
    }
}