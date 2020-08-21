using System;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Models.Account;
using Core.Api.Net.Business.Models.Rbac;
using System.Collections.Generic;

namespace Core.Api.Net.Business.Repository.Auth
{
    public interface IAuthRepository
    {
        SignInResponse SignIn(SignInModel userInfo);
        SignInResponse SignOut(UserInfoResponse userInfo);
        int SignUp(AccountModel account);
        ChangePasswordResponse ChangePassword(ChangePasswordModel info);
        int ForgotPassword(string email);
        string BuildToken(string request_type, string account_id);
        string ReadToken();
        string BuildRegisterToken(RegisterModel register);
        RegisterModel ReadRegisterToken(string token);
        IList<RegisterModel> ClaimRegister(string token);
        string OwnerRegistered(RegisterModel register);
        int UpdateAccountStatus(string request_type, string account_id, string token, DateTime expire_date);
    }
}