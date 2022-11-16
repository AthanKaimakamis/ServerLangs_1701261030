using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Warehouse.Manager.Models;
using Warehouse.Manager.Utils;

namespace Warehouse.Manager.Services
{
  public interface IDbDataScopedService
  {
    Task<string> LoginValidation(Login login);
    Task<string> CreateUser(User user);
  }

  public class DbDataScopedService : DbCommandBase, IDbDataScopedService
  {
    public DbDataScopedService(IOptions<AppConfig> appSettings, ILoggingSingletonService loggingService, IHttpContextAccessor httpContextAccessor) : base(appSettings, loggingService, httpContextAccessor) { }

    public async Task<string> LoginValidation(Login login)
    {
      List<SqlParameter> prms = new ();
      prms.Add(new SqlParameter(Queries.Prms.Login.USERNAME, login.Username));
      prms.Add(new SqlParameter(Queries.Prms.Login.PASSWORD, login.Password));

      try
      {
        return await base.GetScalarResult<string>(Queries.LoginVerify, prms);
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, "0x00010", ex);
        return ex.Message;
      }
    }

    public async Task<string> CreateUser(User user)
    {
      user.Phone = string.IsNullOrEmpty(user.Phone) ? string.Empty : user.Phone;

      List<SqlParameter> prms = new();
      prms.Add(new SqlParameter(Queries.Prms.User.USERNAME, user.Username));
      prms.Add(new SqlParameter(Queries.Prms.User.PASSWORD, user.Password));
      prms.Add(new SqlParameter(Queries.Prms.User.EMAIL, user.EmailAddress));
      prms.Add(new SqlParameter(Queries.Prms.User.PHONE, user.Phone));

      try
      {
        return await base.GetScalarResult<string>(Queries.CreateUser, prms);
      }
      catch(Exception ex)
      {
        loggingService.Log(LogLevel.Error, "0x00011", ex);
        return ex.Message;
      }
    }
  }
}

