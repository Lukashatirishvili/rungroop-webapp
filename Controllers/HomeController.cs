using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}