using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class ClubController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;

    public ClubController(ApplicationDbContext context, IClubRepository clubRepository, IPhotoService photoService) 
    {
        _context = context;
        _clubRepository = clubRepository;
        _photoService = photoService;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        IEnumerable<Club> club = await _clubRepository.GetAll();
        
        return View(club);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);
        return View(club);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel clubVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = result.Url.ToString(),
                Address = new Address
                {
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City,
                    State = clubVM.Address.State,
                }
            };
            _clubRepository.Add(club);
            return RedirectToAction("Index");
        } else
        {
            ModelState.AddModelError("", "Something went wrong");
        }
        
        return View(clubVM);
    }
}