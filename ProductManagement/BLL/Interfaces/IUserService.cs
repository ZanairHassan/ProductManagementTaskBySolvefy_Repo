using DAL.Models;
using DAL.ViewModels;
namespace BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(UserVM userVM);
        Task<User> GetUser(int useerID);
        Task<List<User>> GetAllUser();
        Task<User> UpdateUser(int userID, UserVM userVM);
        Task<User> DeleteUser(int userID);
       Task<User> GetUserByEmail(string email);
        Task<User> GetUserByName(string name);
    }
}
