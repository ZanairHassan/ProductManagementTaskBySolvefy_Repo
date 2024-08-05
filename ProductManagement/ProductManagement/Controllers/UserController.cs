using BLL.Interfaces;
using BLL.Services;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Utilities;

public class UserController : Controller
{
    private readonly TokenUtility _tokenUtility;
    private readonly AsymmetricCryptographyUtility _asymmetricCryptograpyUtility;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public UserController(TokenUtility tokenUtility,
        AsymmetricCryptographyUtility asymmetricCryptographyUtility,
        ITokenService tokenService,
        IUserService userService,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _tokenUtility = tokenUtility;
        _tokenService = tokenService;
        _asymmetricCryptograpyUtility = asymmetricCryptographyUtility;
        _userService = userService;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userService.GetAllUser();
            return View(users);
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> RegisterUser(UserVM user)
    {
        try
        {
            LoggingUtility.LogTxt("RegisterUser action started", _configuration);
            if (!ModelState.IsValid)
            {
                LoggingUtility.LogTxt("Model state is invalid", _configuration);
                return View("Register", user);
            }
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

            user.Email = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(user.Email));
            user.Password = Convert.ToBase64String(_asymmetricCryptograpyUtility.EncryptData(user.Password));
            LoggingUtility.LogTxt("User data encrypted", _configuration);
            var newUser = await _userService.CreateUser(user);

            LoggingUtility.LogTxt("User registered successfully", _configuration);
            return RedirectToAction("Index"); 
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser(LoginVM loginVm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                LoggingUtility.ExcLog(ModelState.ToString(), _serviceProvider);
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByName(loginVm.UserName);
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

            AddTokenVM tokenVM = new AddTokenVM();
            tokenVM.UserId = user.UserId;
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

    [HttpGet]
    public async Task<IActionResult> GetTheUser(int id)
    {
        var findUser = await _userService.GetUser(id);
        if (findUser == null)
        {
            LoggingUtility.LogTxt("user did not exist", _configuration);
            return NotFound();
        }
        try
        {
            LoggingUtility.LogTxt("user has successfully found", _configuration);
            return Ok(findUser);
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.Message, _serviceProvider);
            throw;
        }
    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, UserVM userVM)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var editUser = await _userService.UpdateUser(id, userVM);
                if (editUser == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userVM);
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }
}
