using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace NamSitaKaurLMS.WebUI.Areas.Admin.Controllers
{
    public class AdminAreaAuthorization : IAuthorizationFilter
    {
        //[Obsolete]
        //public void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    var area = context.RouteData.Values["area"]?.ToString();

        //    if (area == "Admin")
        //    {
        //        if (!context.HttpContext.User.Identity.IsAuthenticated ||
        //            !context.HttpContext.User.IsInRole("Admin"))
        //        {
        //            context.Result = new RedirectToActionResult("Login", "Account", null);
        //        }
        //    }
        //}


        public void OnAuthorization(AuthorizationFilterContext authorizationFilterContext)
        {
            var area = authorizationFilterContext.RouteData.Values["area"]?.ToString();
            if (area == "Admin")
            {
                if (!authorizationFilterContext.HttpContext.User.Identity?.IsAuthenticated ?? true)
                {
                    authorizationFilterContext.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }

                if (!authorizationFilterContext.HttpContext.User.IsInRole("Admin"))
                {
                    authorizationFilterContext.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }
        }
    }

}
