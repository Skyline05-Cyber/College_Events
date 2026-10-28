using Microsoft.AspNetCore.Mvc;

namespace CollegeEventHub.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => RedirectToAction("Index", "Events");
    public IActionResult Error() => View();
}
