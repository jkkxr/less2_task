using less2_task.ErrorHandling;
using less2_task.Models;

using Microsoft.EntityFrameworkCore;

namespace less2_task.Services;

public class UserService(ApiDbContext dbContext, IConfiguration configuration)
{
    public async Task<User> RegisterUserAsync(User newUser)
    {
        var registrationAllowed = configuration.GetValue<bool>("RegistrationAllowed");
        if (!registrationAllowed)
        {
            throw new RegistrationDisabledException();
        }

        if (await dbContext.Users
            .AnyAsync(x => x.Email == newUser.Email))
        {
            throw new InvalidDataException("User already exists");
        }

        newUser.Token = GenerateToken(newUser);

        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> AuthenticateUserAsync(string emai, string password)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == emai && u.Password == password);
        return user;
    }

    public string GenerateToken(User user)
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }

    public async Task<User?> ValidateTokenAsync(string token)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Token == token);
    }

    public async Task<ICollection<User>> GetAllUsersAsync()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByEmailIdAsync(string email)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}
