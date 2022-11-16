using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Warehouse.Manager.Models;
using Warehouse.Manager.Services;

namespace Warehouse.Manager.Controllers
{
  public class AuthenticationController : Controller
  {
    private ILoggingSingletonService Logger;
    private IDbDataScopedService DbContext;

    public AuthenticationController(ILoggingSingletonService logger, IDbDataScopedService dbContext)
    {
      Logger = logger;
      DbContext = dbContext;
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> Validate([FromBody] Login login)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      string result = await DbContext.LoginValidation(login);
      if (string.IsNullOrWhiteSpace(result))
        return Redirect("/Home");

      return BadRequest(result);
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> CreateAccount([FromBody] User user)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      string result = await DbContext.CreateUser(user);
      if (string.IsNullOrWhiteSpace(result))
        return Ok("User created successfuly");

      return BadRequest(result);
    }
  }
}
