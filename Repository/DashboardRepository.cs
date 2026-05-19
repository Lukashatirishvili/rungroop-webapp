using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<List<Club>> GetAllUserClubsAsync()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userClubs = await _context.Clubs.Where(x => x.AppUser.Id == currentUserId).ToListAsync();

        return userClubs;
    }
    
    public async Task<List<Race>> GetAllUserRacesAsync()
    {
        var currentUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
        var userRaces = await _context.Races.Where(x => x.AppUser.Id == currentUserId.ToString()).ToListAsync();
        
        return userRaces;
    }
}