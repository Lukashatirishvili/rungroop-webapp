using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
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
        var currentUser = _httpContextAccessor.HttpContext.User.GetUserId();

        var createRaceViewModel = new CreateRaceViewModel
        {
            AppUserId = currentUser,
        };
        
        return View(createRaceViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(raceVM.Image);
            var race = new Race()
            {
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = result.Url.ToString(),
                AppUserId = raceVM.AppUserId,
                Address = new Address
                {
                    Street = raceVM.Address.Street,
                    City = raceVM.Address.City,
                    State = raceVM.Address.State,
                }
            };
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Something went wrong");

        }
        
        return View(raceVM);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var race = await _raceRepository.GetByIdAsync(id);

        if (race == null)
        {
            return NotFound();
        }

        var raceMV = new EditRaceViewModel
        {
            Id = id,
            Title = race.Title,
            Description = race.Description,
            URL = race.Image,
            AddressId = race.AddressId,
            Address = race.Address,
            RaceCategory = race.RaceCategory
        };
        return View(raceMV);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Could not edit race");
            return View(raceVM);
        }

        var userRace = await _raceRepository.GetByIdAsyncNoTracking(id);

        if (userRace != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userRace.Image);
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not delete image");
                return View(raceVM);
            }
            
            var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

            var race = new Race
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = raceVM.AddressId,
                Address = raceVM.Address,
                RaceCategory = raceVM.RaceCategory
            };
            _raceRepository.Update(race);
            return RedirectToAction("Index");
        }
        else
        {
            return View(raceVM);
        }
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var raceDetails = await _raceRepository.GetByIdAsync(id);

        if (raceDetails == null)
        {
            return View("Error");
        }
        
        return View(raceDetails);
    }
    
    
    [HttpPost,  ActionName("Delete")]

    public async Task<IActionResult> DeleteClub(int id)
    {
        var item = await _raceRepository.GetByIdAsync(id);

        if (item == null)
        {
            return View("Error");
        }
        
        _raceRepository.Delete(item);
        return RedirectToAction("Index");
    }
    
}