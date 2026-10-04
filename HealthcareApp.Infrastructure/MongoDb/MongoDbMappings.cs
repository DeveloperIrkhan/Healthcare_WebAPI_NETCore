using HealthcareApp.Domain.Entities.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Healthcare.Infrastructure.MongoDb
{
    public static class MongoDbMappings
    {
        //public static void Register()
        //{
        //    if (!BsonClassMap.IsClassMapRegistered<User>())
        //    {
        //        BsonClassMap.RegisterClassMap<User>(map =>
        //        {
        //            map.AutoMap();

        //            map.MapIdMember(x => x.Id)
        //                .SetSerializer(
        //                    new MongoDB.Bson.Serialization.Serializers.StringSerializer(
        //                        BsonType.ObjectId));

        //            map.MapMember(x => x.Role)
        //                .SetSerializer(
        //                    new MongoDB.Bson.Serialization.Serializers.EnumSerializer<
        //                        Healthcare.Domain.Enums.UserRole>(
        //                            BsonType.String));
        //        });
        //    }

        //    if (!BsonClassMap.IsClassMapRegistered<RefreshToken>())
        //    {
        //        BsonClassMap.RegisterClassMap<RefreshToken>(map =>
        //        {
        //            map.AutoMap();

        //            map.MapIdMember(x => x.Id)
        //                .SetSerializer(
        //                    new MongoDB.Bson.Serialization.Serializers.StringSerializer(
        //                        BsonType.ObjectId));
        //        });
        //    }
        //}
    }
}
