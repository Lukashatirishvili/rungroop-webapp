using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly ApplicationDbContext _context;

    public RaceController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        List<Race> Races = _context.Races.ToList();
        
        return View(Races);
    }

    public IActionResult Detail(int? id)
    {
        Race race = _context.Races.Include(x => x.Address).FirstOrDefault(x => x.Id == id);
        return View(race);
    }
}