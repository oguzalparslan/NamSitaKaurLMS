using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace NamSitaKaurLMS.WebUI.Areas.Admin.Controllers
{
    public class AdminAreaAuthorization : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var area = context.RouteData.Values["area"]?.ToString();

            if (area == "Admin")
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated ||
                    !context.HttpContext.User.IsInRole("Admin"))
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                }
            }
        }
    }

}
