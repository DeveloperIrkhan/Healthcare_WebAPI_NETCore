
using Healthcare.Application.DTOs.Auth;
using Healthcare.Domain.Entities.ApiResponse;

namespace Healthcare.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<APIResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
        Task<APIResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
        Task<APIResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
        Task LogoutAsync(string userId, string refreshToken, CancellationToken cancellationToken = default);
    }
}
