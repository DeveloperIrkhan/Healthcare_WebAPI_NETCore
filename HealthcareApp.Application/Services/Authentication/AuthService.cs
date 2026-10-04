using Healthcare.API.Constants;
using HealthcareApp.Application.Helpers;
using Healthcare.Application.Interfaces.Repositories;
using Healthcare.Application.Interfaces.Services;
using Healthcare.Domain.Entities.ApiResponse;
using Healthcare.Domain.Entities.User;
using Healthcare.Infrastructure.Interfaces.Services;
using Healthcare.Application.DTOs.Auth;
using HealthcareApp.Domain.Entities.User;
using Healthcare.Domain.Enums;

namespace HealthcareApp.Application.Services.Authentication
{
    public class AuthService : ApiResponse, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository,
            IPasswordService passwordService,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;

        }
        private async Task<User?> FindUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await _userRepository
                .GetByRefreshTokenAsync(refreshToken, cancellationToken);
        }
        private async Task<APIResponse<AuthResponse>> GenerateAndReturnTokenForUser(User user, CancellationToken cancellationToken)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var accessTokenExpiresAt = _jwtService.GetAccessTokenExpiration();
            var refreshToken = new RefreshToken
            {
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            // Save the refresh token to the user
            user.RefreshTokens.Add(refreshToken);
            await _refreshTokenRepository.CreateAsync(refreshToken, cancellationToken);
            var authUser = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                User = new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role
                }
            };
            return Ok(authUser, "User logged in successfully");
        }
        public async Task<APIResponse<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var userEmail = request.Email.Trim().ToLowerInvariant();
                if (await _userRepository.ExistsByEmailAsync(userEmail, cancellationToken))
                {
                    return Fail<AuthResponse>("An account with this email already exists");
                }

                var user = new User
                {
                    Email = userEmail,
                    PasswordHash = _passwordService.HashPassword(request.Password),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Role = UserRole.Patient,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _userRepository.CreateUserAsync(user, cancellationToken);

                // Generate access token and refresh token
                return await GenerateAndReturnTokenForUser(user, cancellationToken);

            }
            catch (Exception ex)
            {
                return Fail<AuthResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<APIResponse<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var userEmail = request.Email.Trim().ToLowerInvariant();
                // Check if the user exists
                var user = await _userRepository.GetByEmailAsync(userEmail, cancellationToken);
                if (user == null) return Fail<AuthResponse>("Invalid email or password");
                // Check if the password is correct
                if (!user.IsActive) return Fail<AuthResponse>("User account is inactive");


                // Verify the password
                if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Fail<AuthResponse>("Invalid email or password");
                }


                // Generate access token and refresh token
                return await GenerateAndReturnTokenForUser(user, cancellationToken);
            }
            catch (Exception ex)
            {
                return Fail<AuthResponse>(ex.InnerException?.Message ?? ex.Message);
            }
        }
        public async Task<APIResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
                if (refreshToken == null || !refreshToken.IsActive)
                {
                    return Fail<AuthResponse>("invalid or expired token");
                }
                // We need to find the user containing this token.
                var user = await _userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);

                if (user == null || !user.IsActive)
                {
                    return Fail<AuthResponse>("User account is invalid.");
                }

                refreshToken.RevokedAt = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);
                return await GenerateAndReturnTokenForUser(user, cancellationToken);
            }

            catch (Exception ex)
            {
                return Fail<AuthResponse>(ex.InnerException?.Message ?? ex.Message);
            }

        }


        public async Task LogoutAsync(string userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var storedToken =
             await _refreshTokenRepository.GetByTokenAsync(refreshToken, cancellationToken);
            if (storedToken == null)
            {
                return;
            }

            if (storedToken.UserId != userId)
            {
                return;
            }

            storedToken.RevokedAt = DateTime.UtcNow;

            await _refreshTokenRepository.UpdateAsync(storedToken, cancellationToken);
        }
    }
}
