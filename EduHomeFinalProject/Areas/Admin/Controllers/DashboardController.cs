using Microsoft.AspNetCore.Mvc;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
