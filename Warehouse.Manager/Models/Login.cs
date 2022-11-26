using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Manager.Models
{
  public class Login
  {
    [Required]
    [Display(Name = "Потребителско Име")]
    public string Username { get; set; }

    [Required]
    [Display(Name = "Парола")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberLogin { get; set; }

    public string ReturnUrl { get; set; }
  }
}
