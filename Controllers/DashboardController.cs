using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class DashboardController : Controller
{
    
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var userRaces = await _dashboardRepository.GetAllUserRacesAsync();
        var userClubs = await _dashboardRepository.GetAllUserClubsAsync();

        var dashboardViewModel = new DashboardViewModel
        {
            Races = userRaces,
            Clubs = userClubs
        };
        return View(dashboardViewModel);
    }
}