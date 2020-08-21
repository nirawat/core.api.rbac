using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Account;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.Auth;

namespace Core.Api.Net.Business.Services.Auth
{
    public interface IAuthService
    {
        string RefreshToken(string accont_id);
        SignInResponse SignIn(SignInModel userInfo);
        SignInResponse SignOut(UserInfoResponse userInfo);
        MessageResponse SignUp(AccountModel account);
        ChangePasswordResponse ChangePassword(ChangePasswordModel info);
        MessageResponse ForgotPassword(ForgotPasswordModel email);
        IList<RegisterModel> ClaimRegister(string token);
        ResponseRegister OwnerRegistered(RegisterModel data);
    }
}