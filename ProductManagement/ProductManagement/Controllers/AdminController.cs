using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Utilities;
//[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly AsymmetricCryptographyUtility _asymmetricCryptographyUtility;

    public AdminController(
        IUserService userService,
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        AsymmetricCryptographyUtility asymmetricCryptographyUtility
        )
    {
        _userService = userService;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _asymmetricCryptographyUtility = asymmetricCryptographyUtility;
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
    public async Task<IActionResult> Register(UserVM user)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _userService.GetUserByName(user.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "Username already exists");
                return View(user);
            }

            var existingEmail = await _userService.GetUserByEmail(user.Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(user);
            }

            user.Email = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Email));
            user.Password = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Password));
            // user.Role = "Admin";
            await _userService.CreateUser(user);
            LoggingUtility.LogTxt("The User has successfully registered....", _configuration);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }

    [HttpGet]
   
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            var userVM = new UserVM
            {
                UserName = user.UserName,
                Email = _asymmetricCryptographyUtility.DecryptData(Convert.FromBase64String(user.Email)),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _asymmetricCryptographyUtility.DecryptData(Convert.FromBase64String(user.Password))
            };

            return View(userVM);
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UserVM user)
    {
        try
        {
            user.Email = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Email));
            user.Password = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Password));
            await _userService.UpdateUser(id, user);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await _userService.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUser(id);
            return View(user);
        }
        catch (Exception ex)
        {
            LoggingUtility.ExcLog(ex.ToString(), _serviceProvider);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete()
    {
       
        return RedirectToAction("Index");
    }
}
