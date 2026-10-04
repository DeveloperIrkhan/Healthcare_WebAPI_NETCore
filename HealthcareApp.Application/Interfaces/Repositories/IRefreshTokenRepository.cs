using HealthcareApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Healthcare.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

        Task CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

        Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    }
}
