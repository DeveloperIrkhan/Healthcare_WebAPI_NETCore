using Healthcare.Application.Interfaces.Repositories;
using Healthcare.Domain.Entities.User;
using HealthcareApp.Infrastructure.MongoDb;
using MongoDB.Driver;
namespace HealthcareApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;
        public UserRepository(MongoDbContext mongoDb)
        {
            _user = mongoDb.GetCollection<User>("users");
        }

        public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
        {
            await _user.InsertOneAsync(user, cancellationToken: cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _user
                         .Find(x => x.Email == email.ToLowerInvariant())
                         .AnyAsync(cancellationToken);

        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _user
                         .Find(x => x.Id == id)
                         .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _user
                            .Find(x => x.Email == email.ToLowerInvariant())
                            .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _user.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await _user.ReplaceOneAsync(
                x => x.Id == user.Id,
                user,
                cancellationToken: cancellationToken
            );
        }


        public async Task<User?> GetByRefreshTokenAsync(
    string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _user
                .Find(x => x.RefreshTokens.Any(
                    token => token.Token == refreshToken))
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
