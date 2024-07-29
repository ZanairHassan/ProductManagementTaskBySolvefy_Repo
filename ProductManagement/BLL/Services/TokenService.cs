using BLL.Interfaces;
using DAL.DBContext;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly ApplicationDbContext _context;
        public TokenService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AddToken> CreateToken(AddTokenVM tokenVM)
        {
            AddToken token = new AddToken();
            token.CreatedDate = DateTime.Now;
            token.JwtToken = tokenVM.JwtToken;
            token.UserId = tokenVM.UserId;
            token.IsDeleted=false;
            token.IsExpired=false;
            await _context.AddTokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<AddToken> DeleteToken(int tokenID)
        {
            var token = await _context.AddTokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
            if (token == null)
            {
                return null;
            }
            token.IsDeleted = true;
            _context.AddTokens.Remove(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<List<AddToken>> GetAllToken()
        {
            return await _context.AddTokens.ToListAsync();
        }

        public async Task<AddToken> GetToken(int tokenID)
        {
            return await _context.AddTokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
        }
        public async Task<AddToken> GetTokenByUserId(int UserId)
        {
            return await _context.AddTokens.FirstOrDefaultAsync(x => x.UserId == UserId);
        }

        public async Task<AddToken> UpdateToken(int tokenID, AddTokenVM tokenVM)
        {
            var token = await _context.AddTokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
            if (token != null)
            {
                token.JwtToken = tokenVM.JwtToken;
                _context.AddTokens.Update(token);
                await _context.SaveChangesAsync();
            }

            return token;
        }
        public async Task<AddToken> ValidateToken(string jwtToken)
        {
            var token = await _context.AddTokens.FirstOrDefaultAsync(x => x.JwtToken == jwtToken);
            if (token == null || token.IsDeleted || token.IsExpired)
            {
                return null;
            }

            return token;
        }
    } 
}
