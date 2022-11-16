using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Manager.Models
{
  public class Login
  {
    [Display(Name = "Потребителско Име")]
    [Required]
    public string? Username { get; set; }

    
    [Display(Name = "Парола")]
    [DataType(DataType.Password)]
    [Required]
    public string? Password { get; set; }
  }
}
