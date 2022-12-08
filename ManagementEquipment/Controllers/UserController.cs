using Microsoft.AspNetCore.Mvc;

namespace ManagementEquipment.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult PageUser()
        {
            return View();
        }
    }
}
