namespace Devosmos.ApiStarter.Application.Abstractions.Data;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
