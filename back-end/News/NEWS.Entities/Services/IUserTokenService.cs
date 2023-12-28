using NEWS.Entities.MySqlEntities;

namespace NEWS.Entities.Services
{
    public interface IUserTokenService : IBaseService<UserToken>
    {
        void CacheBlockedTokens();

        void AddCachingBlockedToken(int userId, string token);

        bool IsTokenBlocked(int userId, string token);

        Task BlockAllTokensAsycn(int userId);
    }
}
