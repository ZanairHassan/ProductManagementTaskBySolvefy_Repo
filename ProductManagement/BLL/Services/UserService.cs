using BLL.Interfaces;
using DAL.DBContext;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Utilities;
namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly AsymmetricCryptographyUtility _asymmetricCryptograpyUtility;
        public UserService(ApplicationDbContext Context,
            AsymmetricCryptographyUtility asymmetricCryptographyUtility)
        {
            _context = Context;
            _asymmetricCryptograpyUtility = asymmetricCryptographyUtility;
        }

        public async Task<User> CreateUser(UserVM userVM)
        {
            var user = new User
            {
                UserName = userVM.UserName,
                Password = userVM.Password,
                Email = userVM.Email,
                FirstName = userVM.FirstName,
                LastName = userVM.LastName
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeleteUser(int userID)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userID);
            if (user != null)
            {
                user.IsDeleted = true;
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int userID)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userID);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var emailget = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(email));
            var user= await _context.Users.FirstOrDefaultAsync(x => x.Email == emailget);
            if (user != null)
            {
                return user;
            }
            return user;
        }

        public Task<User> GetUserByName(string name)
        {
            return _context.Users.FirstOrDefaultAsync(x=> x.UserName == name);
        }

        public async Task<User> UpdateUser(int userID, UserVM userVM)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userID);
            if (user != null)
            {
                user.UserName = userVM.UserName;
                user.Password = userVM.Password;
                user.Email = userVM.Email;
                user.FirstName = userVM.FirstName;
                user.LastName = userVM.LastName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
