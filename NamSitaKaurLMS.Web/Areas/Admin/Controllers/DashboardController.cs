using Microsoft.AspNetCore.Mvc;

namespace NamSitaKaurLMS.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
