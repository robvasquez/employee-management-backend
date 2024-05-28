using EmployeeManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly EmployeeContext _context;

    public UserRepository(EmployeeContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}