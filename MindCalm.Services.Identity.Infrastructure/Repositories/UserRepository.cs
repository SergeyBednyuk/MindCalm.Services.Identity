using Microsoft.EntityFrameworkCore;
using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Interfaces;
using MindCalm.Services.Identity.Infrastructure.Data;

namespace MindCalm.Services.Identity.Infrastructure.Repositories;

public class UserRepository(MindCalmIdentityDbContext dbContext) : IUserRepository
{
    private readonly MindCalmIdentityDbContext _dbContext = dbContext;
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(User newUser, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(newUser, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }
}