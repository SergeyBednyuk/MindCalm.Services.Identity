using MindCalm.Services.Identity.Core.Entities;
using MindCalm.Services.Identity.Core.Interfaces;

namespace MindCalm.Services.Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}