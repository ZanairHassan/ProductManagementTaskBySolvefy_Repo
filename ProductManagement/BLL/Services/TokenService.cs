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

        public async Task<Token> CreateToken(TokenVM tokenVM)
        {
            Token token = new Token();
            token.CreatedDate = DateTime.Now;
            token.JwtToken = tokenVM.JwtToken;
            token.UserID = tokenVM.UserID;
            token.IsDeleted=false;
            token.IsExpired=false;
            await _context.Tokens.AddAsync(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<Token> DeleteToken(int tokenID)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
            if (token == null)
            {
                return null;
            }
            token.IsDeleted = true;
            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task<List<Token>> GetAllToken()
        {
            return await _context.Tokens.ToListAsync();
        }

        public async Task<Token> GetToken(int tokenID)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
        }
        public async Task<Token> GetTokenByUserId(int UserId)
        {
            return await _context.Tokens.FirstOrDefaultAsync(x => x.UserID == UserId);
        }

        public async Task<Token> UpdateToken(int tokenID, TokenVM tokenVM)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(x => x.TokenID == tokenID);
            if (token != null)
            {
                token.JwtToken = tokenVM.JwtToken;
                _context.Tokens.Update(token);
                await _context.SaveChangesAsync();
            }

            return token;
        }
        public async Task<Token> ValidateToken(string jwtToken)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(x => x.JwtToken == jwtToken);
            if (token == null || token.IsDeleted || token.IsExpired)
            {
                return null;
            }

            return token;
        }
    } 
}
