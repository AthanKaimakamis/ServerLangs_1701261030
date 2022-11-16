using Warehouse.Manager.Utils;

namespace Warehouse.Manager.Services
{
  public static class Queries
  {
    public static readonly string LoginVerify = $@"
      DECLARE @out_error NVARCHAR(255); 
      EXECUTE {AppConstants.DB_SHEMA}.LoginVerify @username, @password, @out_error OUTPUT; 
      SELECT @out_error error";

    public static readonly string CreateUser = $@"
      DECLARE @out_error NVARCHAR(255);
      EXECUTE {AppConstants.DB_SHEMA}.CreateUser @username, @password, @email, @phone, @out_error OUTPUT;
      SELECT @out_error error";

    public static class Prms
    {
      public static readonly string OUT_ERROR = "@out_error";
      public static class Login
      {
        public static readonly string USERNAME = "@username";
        public static readonly string PASSWORD = "@password";
      }

      public static class User
      {
        public static readonly string USERNAME = "@username";
        public static readonly string PASSWORD = "@password";
        public static readonly string EMAIL = "@email";
        public static readonly string PHONE = "@phone";
      }

    }
  }
}
