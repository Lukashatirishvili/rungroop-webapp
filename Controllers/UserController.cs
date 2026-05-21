using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class UserController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    private readonly IUserRepository _userRepository;

    public UserController(IHttpContextAccessor httpContextAccessor, IPhotoService photoService, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _photoService = photoService;
        _userRepository = userRepository;
    }

    [HttpGet("Users")]
    public async Task<IActionResult> Index()
    {
        var users = await _userRepository.GetAllUsers();
        
        List<UserViewModel> result = new List<UserViewModel>();

        foreach (var user in users)
        {
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl
            };
            result.Add(userViewModel);
        }
        
        return View(result);
    }

    public async Task<IActionResult> Detail(string id)
    {
        var detail = await _userRepository.GetUserById(id);

        var userDetailViewModel = new UserDetailViewModel
        {
            Id = id,
            UserName = detail.UserName,
            Pace = detail.Pace,
            Mileage = detail.Mileage,
        };
        
        return View(userDetailViewModel);
    }
}