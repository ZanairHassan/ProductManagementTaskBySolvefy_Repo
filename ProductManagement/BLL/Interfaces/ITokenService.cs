using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces
{
    public interface ITokenService
    {
        Task<Token> CreateToken(TokenVM tokenVM);
        Task<Token> GetToken(int tokenID);
        Task<Token> GetTokenByUserId(int UserID);
        Task<List<Token>> GetAllToken();
        Task<Token> UpdateToken(int tokenID, TokenVM tokenVM);
        Task<Token> DeleteToken(int tokenID);
       // Task<Token> ValidateToken(string jwtToken);

    }
}
