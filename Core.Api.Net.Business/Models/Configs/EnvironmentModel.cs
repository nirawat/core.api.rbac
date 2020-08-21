namespace Core.Api.Net.Business.Models.Configs
{
    public class EnvironmentModel
    {
        public AppCode AppCodes { get; set; }
        public RegisterCode RegisterCodes { get; set; }
        public DBRBAC DBRBAC { get; set; }
        public DBBusiness DBBusiness { get; set; }
    }
    public class AppCode
    {
        public string JwtCode { get; set; }
    }
    public class RegisterCode
    {
        public string JwtCode { get; set; }
    }
    public class DBRBAC
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class DBBusiness
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public enum DBConnectionType
    {
        RBAC = 0,
        Business = 1,
    }
}