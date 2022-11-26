using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Warehouse.Manager.Models;
using Warehouse.Manager.Utils;

namespace Warehouse.Manager.Services
{
  public interface IDbDataScopedService
  {
    Task<List<ProductType>> GetProductTypes();
    Task<Product> GetProduct(int prodID);
    Task<List<Product>> GetProducts(int? typeID = null, string? name = null);
    Task<string> AddProduct(Product product);
    Task<string> UpdateProduct(Product product);
    Task<string> DeleteProduct(int productID);
  }

  public class DbDataScopedService : DbCommandBase, IDbDataScopedService
  {
    public string ClientIP { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public DbDataScopedService(IOptions<AppConfig> appSettings, ILoggingSingletonService loggingService, IHttpContextAccessor httpContextAccessor) : base(appSettings, loggingService, httpContextAccessor) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    public async Task<List<ProductType>> GetProductTypes()
    {
      List<ProductType> types = new();

      try
      {
        types = await GetListResult<ProductType>(Queries.GetProductTypes());
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, nameof(UpdateProduct), ex, new Dictionary<string, object>
                {
                    { "ClientIPAddress", ClientIP }
                });
      }
      return types;
    }

    public async Task<Product> GetProduct(int prodID)
    {
      var res = new Product();
      string stmt = Queries.GetProduct(prodID);
      try
      {
        res = await GetFirstResult<Product>(stmt);
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, nameof(GetProduct), ex, new Dictionary<string, object>
        {
          { "ClientIPAddress", ClientIP }
        });
      }
      return res;
    }

    public async Task<List<Product>> GetProducts(int? typeID = null, string? name = null)
    {
      var res = new List<Product>();
      List<SqlParameter> prms = new();
      Dictionary<string, string?> wClause = new();
      if (typeID != null)
      {
        wClause.Add(Queries.Prms.Product.TYPEID.Remove(0, 1), typeID.ToString());
      }
      if (!string.IsNullOrWhiteSpace(name))
        wClause.Add(Queries.Prms.Product.NAME.Remove(0, 1), $"'%{name}%'");
      string stmt = Queries.GetProducts(wClause);
      try
      {
        res = await GetListResult<Product>(stmt, prms);
      }
      catch (Exception ex)
      {
        loggingService.Log(LogLevel.Error, nameof(GetProducts), ex, new Dictionary<string, object>
        {
          { "ClientIPAddress", ClientIP }
        });
        throw ex;
      }
      return res;
    }

    public async Task<string> AddProduct(Product product)
    {
      string? res = string.Empty;
      List<SqlParameter> prms = new()
      {
        new SqlParameter(Queries.Prms.Product.TYPEID, product.pType),
        new SqlParameter(Queries.Prms.Product.NAME, product.pName),
        new SqlParameter(Queries.Prms.Product.DESC, product.pDesc),
        new SqlParameter(Queries.Prms.Product.ImgB64, product.pImg),
        new SqlParameter(Queries.Prms.Product.BPRICE, product.pBPrice),
        new SqlParameter(Queries.Prms.Product.SPRICE, product.pSPrice),
        new SqlParameter(Queries.Prms.Product.AMOUNT, product.pAmount),
        new SqlParameter {
          ParameterName = Queries.Prms.OUT_ERROR,
          Size = 255,
          Direction = ParameterDirection.Output}
      };
      try
      {
        await BeginTransaction();
        await ExecuteNonQuery(Queries.SP.ADD_PRODUCT, prms, CommandType.StoredProcedure);
        await CommitTransaction();
      }
      catch (Exception ex)
      {
        await RollbackTransaction();
        loggingService.Log(LogLevel.Error, nameof(AddProduct), ex, new Dictionary<string, object>
        {
          { "ClientIPAddress", ClientIP }
        });
        return ex.Message;
      }
      return product.Error!;
    }

    public async Task<string> UpdateProduct(Product product)
    {
      string? res = string.Empty;
      List<SqlParameter> prms = new()
      {
        new SqlParameter(Queries.Prms.Product.ID, product.pID),
        new SqlParameter(Queries.Prms.Product.TYPEID, product.pType),
        new SqlParameter(Queries.Prms.Product.NAME, product.pName),
        new SqlParameter(Queries.Prms.Product.DESC, product.pDesc),
        new SqlParameter(Queries.Prms.Product.ImgB64, product.pImg),
        new SqlParameter(Queries.Prms.Product.BPRICE, product.pBPrice),
        new SqlParameter(Queries.Prms.Product.SPRICE, product.pSPrice),
        new SqlParameter(Queries.Prms.Product.AMOUNT, product.pAmount),
        new SqlParameter {
          ParameterName = Queries.Prms.OUT_ERROR,
          Size = 255,
          Direction = ParameterDirection.Output}
      };
      try
      {
        await BeginTransaction();
        await ExecuteNonQuery(Queries.SP.UPDATE_PRODUCT, prms, CommandType.StoredProcedure);
        await CommitTransaction();
        res = prms.Last().Value.ToString();
      }
      catch (Exception ex)
      {
        try { await RollbackTransaction(); } catch { }
        loggingService.Log(LogLevel.Error, nameof(UpdateProduct), ex, new Dictionary<string, object>
        {
          { "ClientIPAddress", ClientIP }
        });
        return ex.Message;
      }
      return res;
    }

    public async Task<string> DeleteProduct(int productID)
    {
      string? res = string.Empty;
      List<SqlParameter> prms = new()
      {
        new SqlParameter(Queries.Prms.Product.ID, productID),
        new SqlParameter {
          ParameterName = Queries.Prms.OUT_ERROR,
          Size = 255,
          Direction = ParameterDirection.Output}
      };
      try
      {
        await BeginTransaction();
        await ExecuteNonQuery(Queries.SP.DELETE_PRODUCT, prms, CommandType.StoredProcedure);
        await CommitTransaction();
        res = prms.Last().Value.ToString();
      }
      catch (Exception ex)
      {
        try { await RollbackTransaction(); } catch { }
        loggingService.Log(LogLevel.Error, nameof(DeleteProduct), ex, new Dictionary<string, object>
        {
          { "ClientIPAddress", ClientIP }
        });
        return ex.Message;
      }
      return res;
    }
  }
}

