namespace HealthcareApp.Domain.Entities.User
{
    public class RefreshToken
    {
        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsExpired =>
            DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked =>
            RevokedAt.HasValue;

        public bool IsActive =>
            !IsExpired && !IsRevoked;
    }
}
