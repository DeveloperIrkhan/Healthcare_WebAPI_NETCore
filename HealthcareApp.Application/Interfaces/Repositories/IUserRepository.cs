using Healthcare.Domain.Entities.User;

namespace Healthcare.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task CreateUserAsync(User user, CancellationToken cancellationToken);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
