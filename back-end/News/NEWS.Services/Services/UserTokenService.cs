using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEWS.Entities.Models.Others;
using NEWS.Entities.MySqlEntities;
using NEWS.Entities.Services;
using NEWS.Entities.UnitOfWorks;
using NEWS.Entities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Services.Services
{
    public class UserTokenService : BaseService<UserToken>, IUserTokenService
    {
        public UserTokenService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        public void CacheBlockedTokens()
        {
            var token = _repository.GetAll(_ => _.IsBlocked)
                .AsNoTracking()
                .GroupBy(_ => _.UserId)
                .Select(x => new BlockedToken {
                    UserId = x.Key,
                    Tokens = x.Select(_ => _.Token).ToList()
                })
                .ToList();

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

        public async Task BlockAllTokensAsycn(int userId)
        {
            var userTokens = await _repository.GetAll(_ => _.UserId == userId
                && !_.IsBlocked).ToListAsync();

            userTokens.ForEach(_ => _.IsBlocked = true);
            _unitOfWork.DbContext.UpdateRange(userTokens);
            await _unitOfWork.SaveChangesAsync();

            AddCachingBlockedTokens(userId, userTokens.Select(_ => _.Token).ToList());
        }
    }
}
