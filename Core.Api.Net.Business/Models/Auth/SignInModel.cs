using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Auth
{
    public class SignInModel
    {
        public string ipAddress { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }
    }

    public class SignInResponse
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public string token { get; set; }
        public UserInfoResponse userInfo { get; set; }
    }
    public class UserInfoResponse
    {
        public string accountId { get; set; }
        public string groupname { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string companyName { get; set; }
        public string language { get; set; }
        public string theme { get; set; }
        public byte[] avatar { get; set; }
        public bool remember { get; set; }
        public FuncListModel funcList { get; set; }
        public OwnerModel owner { get; set; }
        public RegisterModel register { get; set; }

    }

    public class ChangePasswordModel
    {
        public string account_id { get; set; }
        public string email { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
    }

        public class ChangePasswordResponse
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
    }
}