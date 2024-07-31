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
        var users = await _userService.GetAllUser();
        return View(users);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserVM user)
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
        user.UserType = "Admin";
        await _userService.CreateUser(user);
        LoggingUtility.LogTxt("The User has successfully registered....", _configuration);
        return RedirectToAction("Index");
    }

    [HttpGet]
   
    public async Task<IActionResult> Edit(int id)
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

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UserVM user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        user.Email = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Email));
        user.Password = Convert.ToBase64String(_asymmetricCryptographyUtility.EncryptData(user.Password));
        await _userService.UpdateUser(id, user);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);
        return RedirectToAction("Index");
    }
}
