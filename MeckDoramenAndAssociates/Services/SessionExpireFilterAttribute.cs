using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace MeckDoramenAndAssociates.Services
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("EDnAloggedinuser") == null ||
                filterContext.HttpContext.Session.GetInt32("EDnAloggedinuserid") == null)
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Account"},
                            {"action", "SignIn"},
                            {"returnUrl", "sessionExpired"},
                        });


            base.OnActionExecuting(filterContext);
        }
    }
}
