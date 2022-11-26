using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Warehouse.Manager.Models;
using Warehouse.Manager.Services;

namespace Warehouse.Manager.Controllers
{
  [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
  public class WarehouseController : Controller
  {
    private IDbDataScopedService context;

    public WarehouseController(IDbDataScopedService db)
    {
	  context = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetTypes()
    {
      return Ok(HttpContext.User.Identity.Name);
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? typeID, string? name)
    {
      List<Product> products = await context.GetProducts(typeID, name);
      return Ok(HttpContext.User.Identity.Name);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
      string res = await context.AddProduct(product);
      if (string.IsNullOrWhiteSpace(res))
        return Redirect(nameof(AddProduct));
      return BadRequest(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
      string res = await context.UpdateProduct(product);
      if (string.IsNullOrWhiteSpace(res))
        return RedirectToAction(nameof(Index), new { name = product.pName});
      return BadRequest(res);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveProduct([FromForm] int id)
    {
      string res = await context.DeleteProduct(id);
      var prods = new List<Product>();
      if (string.IsNullOrWhiteSpace(res))
        return Redirect(nameof(Index));
      return BadRequest(res);
    }
  }
}

