using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace AuditApp.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly TokenUtility _tokenUtility;
        private readonly AsymmetricCryptographyUtility _asymmetricCryptograpyUtility;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        // private readonly  LoggingUtlity _logger;

        public UserController(TokenUtility tokenUtility,
            AsymmetricCryptographyUtility asymmetricCryptographyUtility,
            ITokenService tokenService,
            AsymmetricCryptographyUtility asymmetricCryptograpyUtility,
            IUserService userService,
            IConfiguration configuration,
            IServiceProvider serviceProvider
            //LoggingUtlity logger
            )
        {
            _tokenUtility = tokenUtility;
            _tokenService = tokenService;
            _asymmetricCryptograpyUtility = asymmetricCryptograpyUtility;
            _userService = userService;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            //  _logger = logger;
        }

        // Other controller methods

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserVM user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email))
                {
                    LoggingUtility.LogTxt("User data is null or username/email is null", _configuration);
                    return BadRequest("User data is null maybe username or Email is null");
                }

                var existingUser = await _userService.GetUserByName(user.UserName);
                if (existingUser != null)
                {
                    LoggingUtility.LogTxt("Username already exists", _configuration);
                    return Conflict("Username already exists");
                }

                var existingEmail = await _userService.GetUserByEmail(user.Email);
                if (existingEmail != null)
                {
                    LoggingUtility.LogTxt("Email already exists", _configuration);
                    return Conflict("Email already exists");
                }

                if (user != null)
                {
                    var token = await _tokenUtility.GenerateToken(user.Email);
                    TokenVM tokenVM = new TokenVM();
                    tokenVM.JwtToken = token;
                    var tkon = await _tokenService.CreateToken(tokenVM);
                   
                    user.Email = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(user.Email));
                    user.Password = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(user.Password));
                    _userService.CreateUser(user);
                    LoggingUtility.LogTxt("User registered successfully", _configuration);
                    return Ok(tkon);
                }
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM loginVm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                    return BadRequest(ModelState);
                }

                var user = await _userService.GetUserByName(loginVm.UserName);
                //var user = allUsers.FirstOrDefault(x => _asymmetricCryptograpyUtility.DecryptData(Convert.FromBase64String(x.Email)) == loginVm.Email);

                if (user == null)
                {
                    LoggingUtility.LogTxt("User not found", _configuration);
                    return NotFound("User not found");
                }

                if (loginVm.Password != _asymmetricCryptograpyUtility.DecryptData(Convert.FromBase64String(user.Password)))
                {
                    LoggingUtility.LogTxt("Invalid password", _configuration);
                    return Unauthorized("Invalid password");
                }

                string email = _asymmetricCryptograpyUtility.DecryptData(Convert.FromBase64String(user.Email));

                var token = await _tokenUtility.GenerateToken(email);

                TokenVM tokenVM = new TokenVM();
                tokenVM.UserID = user.UserId;
                tokenVM.JwtToken = token;
                var tkon = await _tokenService.CreateToken(tokenVM);
                LoggingUtility.LogTxt("User logged in successfully", _configuration);
                return Ok(tkon);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
                throw;
            }
        }

        ////******************************* Forget Password ****************
        //[HttpPost("ForgetPassword")]
        //public async Task<IActionResult> ForgetPassword(ForgetPasswordVM eModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        LoggingUtlity.ExLog(ModelState.ToString(), _serviceProvider);
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _userService.GetUserByName(eModel.Username);
        //    if (user == null)
        //    {
        //        LoggingUtility.LogTxt("\"user not exist with this email", _configuration);
        //        return BadRequest("user not exist with this email");
        //    }
        //    var dectypted_email = _asymmetricCryptograpyUtility.DecryptData(Convert.FromBase64String(user.Email));
        //    try
        //    {
        //        string temporyPassword = GenerateTemporaryPassword();
        //        var password = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(temporyPassword));
        //        UserVM usersVM = new UserVM();

        //        usersVM.FirstName = user.FirstName;
        //        usersVM.LastName = user.LastName;
        //        usersVM.UserName = user.UserName;
        //        usersVM.Email = user.Email;
        //        usersVM.Password = password;
        //        await _userService.UpdateUser(user.UserID, usersVM);
        //        EmailSentForForgetPassword.SendEmailForPassword(dectypted_email,temporyPassword);
        //        SMSTypeRequest.SendSMS(user.PhoneNo,temporyPassword);

        //        return Ok("THE password update request has successfully sent");
                
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingUtlity.ExLog(ex.Message, _serviceProvider);
        //        throw;
        //    }
        //    }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetTheUser(int id)
        {
            var findUser=await _userService.GetUser(id);
            if(findUser == null)
            {
                LoggingUtility.LogTxt("user didnot exist",_configuration);
                return NotFound();
            }
            try
            {
                LoggingUtility.LogTxt("user has successfully found", _configuration);
                return Ok(findUser);
            }
            catch (Exception ex)
            {
                LoggingUtility.ExcLog(ex.Message,_serviceProvider);
                throw;
            }
        }
        private string GenerateTemporaryPassword()
        {
            return Guid.NewGuid().ToString();
        }
        // Other controller methods
    }
}
