using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Manager.Models;
using Warehouse.Manager.Services;

namespace Warehouse.Manager.Controllers
{
  public class AuthController : Controller
  {
    private ILoggingSingletonService Logger;
    private IAuthenticationScopedService Auth;

    public AuthController(ILoggingSingletonService logger, IAuthenticationScopedService auth)
    {
      Logger = logger;
      Auth = auth;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
      Login login = new();
      login.ReturnUrl = returnUrl;
      return View(login);
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([FromForm] Login login)
    {
      if (!ModelState.IsValid)
        return View(login);

      string result = await Auth.LoginUser(HttpContext, login);
      if (!string.IsNullOrWhiteSpace(result))
      {
        ViewBag.Message = result;
        return View(login);
      }

      return LocalRedirect(login.ReturnUrl);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
      return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAccount([FromBody] User user)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      string result = await Auth.CreateUser(user);
      if (string.IsNullOrWhiteSpace(result))
        return Ok("User created successfuly");

      return BadRequest(result);
    }
  }
}
