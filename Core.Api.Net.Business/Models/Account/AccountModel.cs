namespace Core.Api.Net.Business.Models.Account
{
    public class AccountModel
    {
        public string account_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string prefixes { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

        public class ForgotPasswordModel
    {
        public string email { get; set; }
    }
}