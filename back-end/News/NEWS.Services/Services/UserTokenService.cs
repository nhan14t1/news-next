using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Extensions;
using NEWS.Entities.Models.Others;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Entities.Utils;

namespace NEWS.Services.Services
{
    public class UserTokenService : BaseService<UserToken>, IUserTokenService
    {
        public UserTokenService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public async Task CacheBlockedTokensAsync()
        {
            var token = await _repository.GetAll(_ => _.IsBlocked)
                .AsNoTracking()
                .GroupBy(_ => _.UserId)
                .Select(x => new BlockedToken {
                    UserId = x.Key,
                    Tokens = x.Select(_ => _.Token).ToList()
                })
                .ToListAsync();

            TokenUtils.BLOCKED_TOKENS = token;
        }

        public void AddCachingBlockedToken(int userId, string token)
        {
            var item = TokenUtils.BLOCKED_TOKENS.FirstOrDefault(x => x.UserId == userId);
            if (item != null)
            {
                item.Tokens.Add(token);
            }
            else
            {
                item = new BlockedToken
                { 
                    UserId = userId,
                    Tokens= new List<string> { token }
                };
            }
        }

        private void AddCachingBlockedTokens(int userId, List<string> tokens)
        {
            var item = TokenUtils.BLOCKED_TOKENS.FirstOrDefault(x => x.UserId == userId);
            if (item != null)
            {
                item.Tokens.AddRange(tokens);
            }
            else
            {
                item = new BlockedToken
                {
                    UserId = userId,
                    Tokens = tokens
                };
                TokenUtils.BLOCKED_TOKENS.Add(item);
            }
        }

        public bool IsTokenBlocked(int userId, string token)
        {
            if (userId == 0)
            {
                return false;
            }

            return TokenUtils.BLOCKED_TOKENS.Any(_ => _.UserId == userId
                && _.Tokens.Contains(token));
        }

        public async Task BlockAllTokensAsync(int userId)
        {
            var userTokens = await _repository.GetAll(_ => _.UserId == userId
                && !_.IsBlocked).ToListAsync();

            userTokens.ForEach(_ => _.IsBlocked = true);
            _unitOfWork.DbContext.UpdateRange(userTokens);
            await _unitOfWork.SaveChangesAsync();

            AddCachingBlockedTokens(userId, userTokens.Select(_ => _.Token).ToList());
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTime.Now.ToTimeStamp();
            var tokens = await _repository.GetAll(_ => _.ExpirationDate < now)
                .ToListAsync();
            _repository.DbContext.RemoveRange(tokens);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task BlockTokenAsync(int userId, string token)
        {
            var userToken = await _repository.GetAll(_ => _.UserId == userId
                && _.Token == token).FirstOrDefaultAsync();
            if (userToken != null)
            {
                userToken.IsBlocked = true;
                await _repository.UpdateAsync(userToken);
            }
        }
    }
}
