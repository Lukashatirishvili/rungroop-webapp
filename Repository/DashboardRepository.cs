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

    public async Task<AppUser> GetUserById(string id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByIdNoTracking(string id)
    {
        return await _context.Users.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }

    public bool Update(AppUser user)
    {
        _context.Users.Update(user);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0;  
    }
}