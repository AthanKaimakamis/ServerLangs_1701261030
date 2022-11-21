using Warehouse.Manager.Utils;

namespace Warehouse.Manager.Services
{
  public static class Queries
  {
    public static readonly string LoginVerify = $@"
      {SP.LOGIN} 
      {Prms.User.USERNAME}, {Prms.User.PASSWORD}, {Prms.OUT_ERROR} OUT";

    public static readonly string CreateUser = $@"
      {SP.CREATEUSER}
      {Prms.User.USERNAME}, {Prms.User.PASSWORD}, {Prms.User.EMAIL}, {Prms.User.PHONE}, {Prms.OUT_ERROR} OUT";

    public static readonly string GetAllProducts = $@"
      SELECT * FROM {V.PRODUCTS}";

    public static readonly string AddProductType = $@"
      {SP.ADD_PRODUCT_TYPE}
      {Prms.ProdType.NAME}, {Prms.OUT_ERROR} OUT";

    public static readonly string AddProduct = $@"
      {SP.ADD_PRODUCT}
      {Prms.Product.TYPEID},
      {Prms.Product.NAME},
      {Prms.Product.DESC},
      {Prms.Product.ImgB64},
      {Prms.Product.BPRICE},
      {Prms.Product.SPRICE},
      {Prms.Product.AMOUNT},
      {Prms.OUT_ERROR} OUT";

    public static readonly string UpdateProduct = $@"
      {SP.UPDATE_PRODUCT}
      {Prms.Product.ID},
      {Prms.Product.TYPEID},
      {Prms.Product.NAME},
      {Prms.Product.DESC},
      {Prms.Product.ImgB64},
      {Prms.Product.BPRICE},
      {Prms.Product.SPRICE},
      {Prms.Product.AMOUNT},
      {Prms.OUT_ERROR} OUT";

    public static readonly string DeleteProduct = @$"
    {SP.DELETE_PRODUCT}
    {Prms.Product.ID}, {Prms.OUT_ERROR} OUT";

    public static string GetProduct(List<string>? colums = null, Dictionary<string,string>? wClause = null)
    {
      string stmt = "SELECT";
      if (colums != null)
      {
        foreach (string colum in colums)
        {
          stmt += $" {colum},";
        }
        stmt.Remove(stmt.Length);
      }
      else
        stmt += " * ";

      stmt += $" FROM {V.PRODUCTS}  WHERE 1=1";

      if (wClause != null)
      {
        var en = wClause.GetEnumerator();
        while(en.MoveNext())
          stmt += string.Format(" AND {0} LIKE {1}", en.Current.Key , en.Current.Value);
      }
      return stmt;
    }
    public static class SP
    {
      public const string LOGIN = $"{AppConstants.DB_SHEMA}.SP_LoginVerify";
      public const string CREATEUSER = $"{AppConstants.DB_SHEMA}.SP_CreateUser";
      public const string ADD_PRODUCT_TYPE = $"{AppConstants.DB_SHEMA}.SP_AddProductType";
      public const string ADD_PRODUCT = $"{AppConstants.DB_SHEMA}.SP_AddProduct";
      public const string UPDATE_PRODUCT = $"{AppConstants.DB_SHEMA}.SP_UpdateProduct";
      public const string DELETE_PRODUCT = $"{AppConstants.DB_SHEMA}.SP_DeleteProduct";
    }
    public static class T
    {
      public const string PRODUCTS = $"{AppConstants.DB_SHEMA}.T_Products";
    }
    public static class V
    {
      public const string PRODUCTS = $"{AppConstants.DB_SHEMA}.V_Products";
    }
    public static class Prms
    {
      public static readonly string OUT_ERROR = "@out_error";
      public static class User
      {
        public const string USERNAME = "@username";
        public const string PASSWORD = "@password";
        public const string EMAIL = "@email";
        public const string PHONE = "@phone";
      }
      public static class Product
      {
        public const string ID = "@pID";
        public const string TYPEID = "@pTypeId";
        public const string NAME = "@pName";
        public const string DESC = "@pDscription";
        public const string ImgB64 = "@pImageB64";
        public const string BPRICE = "@pBoughtPrice";
        public const string SPRICE = "@pSellPrice";
        public const string AMOUNT = "@pAmount";
      }
      public static class ProdType
      {
        public const string NAME = "@Name";
      }

    }
  }
}
