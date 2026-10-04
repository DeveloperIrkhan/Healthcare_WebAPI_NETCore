
using Healthcare.Domain.Entities.User;

namespace Healthcare.Infrastructure.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        DateTime GetAccessTokenExpiration();

        string GenerateRefreshToken();
    }
}
