using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IRaceRepository _raceRepository;

    public RaceController(ApplicationDbContext context, IRaceRepository raceRepository)
    {
        _context = context;
        _raceRepository = raceRepository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        IEnumerable<Race> races = await _raceRepository.GetAll();        
        return View(races);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Race race = await _raceRepository.GetByIdAsync(id);
        return View(race);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Race race)
    {
        if (!ModelState.IsValid)
        {
            return View(race);
        }
        
        _raceRepository.Add(race);
        return RedirectToAction("Index");
    }
}