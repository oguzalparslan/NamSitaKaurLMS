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


        public void OnAuthorization(AuthorizationFilterContext ctx)
        {
            var area = ctx.RouteData.Values["area"]?.ToString();

            if (area == "Admin")
            {
                if (!(ctx.HttpContext.User.Identity?.IsAuthenticated ?? false))
                {
                    ctx.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
                    return;
                }

                if (!ctx.HttpContext.User.IsInRole("Admin"))
                {
                    ctx.Result = new RedirectToActionResult("AccessDenied", "Account", new { area = "" });
                    return;
                }
            }
        }

    }

}
