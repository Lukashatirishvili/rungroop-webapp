using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;

namespace RunGroopWebApp.Controllers;

public class ClubController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClubController(ApplicationDbContext context) 
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        
        var clubs = _context.Clubs.ToList();
        return View(clubs);
    }
}