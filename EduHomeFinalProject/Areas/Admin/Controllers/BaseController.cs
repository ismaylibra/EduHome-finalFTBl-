using EduHomeFinalProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeFinalProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =Constants.AdminRole)]
    public class BaseController : Controller
    {
       
    }
}
