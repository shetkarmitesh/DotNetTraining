using Microsoft.AspNetCore.Mvc;

namespace VisitorSecurityClearanceSystem.Controllers
{
    public class SecurityUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
