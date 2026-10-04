using Healthcare.Domain.Enums;
using HealthcareApp.Domain.Entities.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Healthcare.Domain.Entities.User
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Patient;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
