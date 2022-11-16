using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Warehouse.Manager.Models
{
  public class User
  {
    [Display(Name = "Email адрес")]
    [Required(ErrorMessage = "Задължително поле!"), EmailAddress(ErrorMessage = "Не валиден email адрес!")]
    public string EmailAddress { get; set; }

    [Display(Name = "Потребителско Име")]
    [Required(ErrorMessage = "Задължително поле!")]
    [StringLength(maximumLength: 15, ErrorMessage = "Потребителското име може да е между 5 и 15 симбола!", MinimumLength = 5)]
    public string Username { get; set; }

    [Display(Name = "Парола")]
    [Required(ErrorMessage = "Задължително поле!"), PasswordPropertyText]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@\-_~|])[A-Za-z\d@\-_~|]{6,20}$", ErrorMessage = @"Паролата не спазва изискванията!")]
    public string Password { get; set; }

    [Display(Name = "Потвърждаване на парола")]
    [Required(ErrorMessage = "Задължително поле!"), PasswordPropertyText]
    [Compare("Password",ErrorMessage = "Паролите не съвпадат!")]
    public string PasswordMatch { get; set; }

    [Phone,DefaultValue(null)]
    [RegularExpression(@"^(\+\d{1,3}\s)?\(?\d{3,4}\)?[\s.-]?\d{3}[\s.-]?\d{3,4}", ErrorMessage = "Невалиден номер!")]
    public string? Phone { get; set; }

  }
}
