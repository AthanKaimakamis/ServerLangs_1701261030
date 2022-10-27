using System;
using Microsoft.Extensions.Options;
using Warehouse.Manager.Models;
using Warehouse.Manager.Utils;

namespace Warehouse.Manager.Services
{
    public interface IDbDataScopedService
    {
        Task<List<User>> GetUser();
    }

    public class DbDataScopedService : DbCommandBase, IDbDataScopedService
    {
        public DbDataScopedService(IOptions<AppConfig> appSettings, ILoggingSingletonService loggingService, IHttpContextAccessor httpContextAccessor) : base(appSettings, loggingService, httpContextAccessor) { }

        public async Task<List<User>> GetUser()
        {
            return await base.GetListResult<User>("select u.sid,u.username from [pb].[Users] u");
        }
    }
}

