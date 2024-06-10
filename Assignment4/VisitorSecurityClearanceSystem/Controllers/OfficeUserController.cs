using Microsoft.AspNetCore.Mvc;

namespace VisitorSecurityClearanceSystem.Controllers
{
    public class OfficeUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
