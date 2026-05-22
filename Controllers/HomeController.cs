using System.Diagnostics;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RunGroopWebApp.Helpers;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class HomeController : Controller
{
    private readonly IClubRepository _clubRepository;

    public HomeController(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }
    public async Task<IActionResult> Index()
    {
        
        var ipInfo = new IPInfo();
        var homeViewModel = new HomeViewModel();

        try
        {
            string url = "https://api.ipinfo.io?token=68ae1fa46d2d10";
            var info = new WebClient().DownloadString(url);
            ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
            RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
            ipInfo.Country = myRI1.EnglishName;
            homeViewModel.City = ipInfo.City;
            homeViewModel.State = ipInfo.Region;

            if (homeViewModel.City == null)
            {
                homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
            }
            else
            {
                homeViewModel.Clubs = null;
            }
            return View(homeViewModel);
        }
        catch (Exception ex)
        {
            homeViewModel.Clubs = null;
        }
        
        return View(homeViewModel);
    }
}