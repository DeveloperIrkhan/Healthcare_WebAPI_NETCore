using Healthcare.Application.Interfaces.Repositories;
using HealthcareApp.Domain.Entities.User;
using HealthcareApp.Infrastructure.MongoDb;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly IMongoCollection<RefreshToken> _tokens;

        public RefreshTokenRepository(MongoDbContext mongoDb)
        {
            _tokens = mongoDb.GetCollection<RefreshToken>("refreshtokens");
        }



        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _tokens
                .Find(x => x.Token == token)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _tokens.InsertOneAsync(refreshToken, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _tokens.ReplaceOneAsync(  x => x.Id == refreshToken.Id,
                refreshToken,
                cancellationToken: cancellationToken);
        }
    }
}
