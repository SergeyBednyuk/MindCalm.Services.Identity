using MindCalm.Services.Identity.Core.Interfaces;
using MindCalm.Services.Identity.Infrastructure.Data;

namespace MindCalm.Services.Identity.Infrastructure.Repositories;

public class UnitOfWork(MindCalmIdentityDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}