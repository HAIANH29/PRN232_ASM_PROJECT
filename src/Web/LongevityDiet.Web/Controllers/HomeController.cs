using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LongevityDiet.Web.Models;
using LongevityDiet.Web.Services;

namespace LongevityDiet.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserSession _session;

    public HomeController(ILogger<HomeController> logger, UserSession session)
    {
        _logger = logger;
        _session = session;
    }

    public IActionResult Index()
    {
        // Admins work only in the admin screens, not the member dashboard.
        if (_session.IsAdmin)
        {
            return RedirectToAction("Index", "AdminKnowledge");
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
