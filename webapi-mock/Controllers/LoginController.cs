using Microsoft.AspNetCore.Mvc;

namespace webapi_mock.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
