using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Principal;
using Warehouse.Manager.Models;
using Microsoft.Extensions.Options;
using Warehouse.Manager.Utils;
using System.Data;
using System.Data.SqlClient;
using static Warehouse.Manager.Services.Queries.Prms;

namespace Warehouse.Manager.Services
{
  public interface IAuthenticationScopedService
  {
    Task<string> LoginUser(HttpContext context, Login login);
    Task<string> CreateUser(Models.User user);
  }

  public class AuthenticationScopedService : DbCommandBase, IAuthenticationScopedService
  {
    public string ClientIP { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public AuthenticationScopedService(IOptions<AppConfig> appSettings, ILoggingSingletonService loggingService, IHttpContextAccessor httpContextAccessor) : base(appSettings, loggingService, httpContextAccessor) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private async Task<bool> SignInUser(HttpContext context, Login login)
    {
      try
      {
        UserIdentity userIdentity = await GetFirstResult<UserIdentity>(Queries.GetUser(login.Username));
        var principal = new ClaimsPrincipal(IdentityService.GetIdentity(userIdentity));
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
        {
          IsPersistent = login.RememberLogin
        });
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, nameof(SignInUser), ex, new Dictionary<string, object>
                {
                    { "ClientIPAddress", ClientIP }
                });
      }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
      return context.User.Identity.IsAuthenticated;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }

    public async Task<string> LoginUser(HttpContext context, Login login)
    {
      string? res = string.Empty;
      List<SqlParameter> prms = new() {
                new SqlParameter(Queries.Prms.User.USERNAME, login.Username),
                new SqlParameter(Queries.Prms.User.PASSWORD, login.Password),
                new SqlParameter(){
                    ParameterName = Queries.Prms.OUT_ERROR,
                    Size = 255,
                    Direction = ParameterDirection.Output}
                };
      try
      {
        await ExecuteNonQuery(Queries.SP.LOGIN, prms, CommandType.StoredProcedure);
        res = prms.Last().Value.ToString();

        if (string.IsNullOrWhiteSpace(res))
          if (!await SignInUser(context, login))
            throw new ("Проблем с автентикацията. Моля обърнете се към администратор");
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, nameof(LoginUser), ex, new Dictionary<string, object>
                {
                    { "ClientIPAddress", ClientIP }
                });
        res = ex.Message;
      }
      return res;
    }

    public async Task<string> CreateUser(Models.User user)
    {
      string? res = string.Empty;
      user.Phone = string.IsNullOrEmpty(user.Phone) ? string.Empty : user.Phone;
      List<SqlParameter> prms = new()
            {
            new SqlParameter(Queries.Prms.User.USERNAME, user.Username),
            new SqlParameter(Queries.Prms.User.PASSWORD, user.Password),
            new SqlParameter(Queries.Prms.User.EMAIL, user.EmailAddress),
            new SqlParameter(Queries.Prms.User.PHONE, user.Phone),
            new SqlParameter(){
                ParameterName = Queries.Prms.OUT_ERROR,
                Size = 255,
                Direction = ParameterDirection.Output}
            };
      try
      {
        await BeginTransaction();
        await ExecuteNonQuery(Queries.SP.CREATEUSER, prms, CommandType.StoredProcedure);
        await CommitTransaction();
        res = prms.Last().Value.ToString();
      }
      catch (Exception ex)
      {
        await RollbackTransaction();
        loggingService.Log(LogLevel.Error, nameof(CreateUser), ex, new Dictionary<string, object>
                {
                    { "ClientIPAddress", ClientIP }
                });
        res = ex.Message;
      }
      return res;
    }
  }
}

