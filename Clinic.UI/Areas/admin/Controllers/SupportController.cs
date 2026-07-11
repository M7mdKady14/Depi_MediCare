using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
