using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces
{
    public interface ITokenService
    {
        Task<AddToken> CreateToken(AddTokenVM tokenVM);
        Task<AddToken> GetToken(int tokenID);
        Task<AddToken> GetTokenByUserId(int UserID);
        Task<List<AddToken>> GetAllToken();
        Task<AddToken> UpdateToken(int tokenID, AddTokenVM tokenVM);
        Task<AddToken> DeleteToken(int tokenID);
       // Task<Token> ValidateToken(string jwtToken);

    }
}
