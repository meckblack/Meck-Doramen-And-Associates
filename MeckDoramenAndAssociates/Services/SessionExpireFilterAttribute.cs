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
            if (filterContext.HttpContext.Session.GetString("MDnAloggedinuser") == null ||
                filterContext.HttpContext.Session.GetInt32("MDnAloggedinuserid") == null ||
                filterContext.HttpContext.Session.GetInt32("MDnAloggedinuserroleid") == null)
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
