using System;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.Auth;
using Core.Api.Net.Business.Models.Account;
using Core.Api.Net.Business.Helpers.Logs;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Repository.Logs;

namespace Core.Api.Net.Business.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogs _ILogs;
        private readonly IAuthRepository _IAuthRepository;
        private readonly ILogRepository _ILogRepository;
        public AuthService(ILogs ILogs, IAuthRepository IAuthRepository, ILogRepository ILogRepository)
        {
            _ILogs = ILogs;
            _IAuthRepository = IAuthRepository;
            _ILogRepository = ILogRepository;
        }

        public string RefreshToken(string accont_id)
        {
            try
            {
                string resp = _IAuthRepository.BuildToken("Request Token", accont_id);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Refresh Token Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return "";
        }

        public SignInResponse SignIn(SignInModel userInfo)
        {
            SignInResponse resp = new SignInResponse();
            try
            {
                resp = _IAuthRepository.SignIn(userInfo);

                if (resp != null && resp.userInfo != null)
                {
                    resp.status = resp.status;
                    resp.message = resp.message;
                    resp.token = _IAuthRepository.BuildToken("Sign In", resp.userInfo.accountId);
                    resp.userInfo.avatar = null;
                    resp.userInfo.remember = userInfo.remember;
                }

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignIn Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public SignInResponse SignOut(UserInfoResponse userInfo)
        {
            SignInResponse resp = new SignInResponse();
            try
            {
                resp = _IAuthRepository.SignOut(userInfo);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignOut Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordModel info)
        {
            ChangePasswordResponse resp = new ChangePasswordResponse();
            try
            {
                if (string.IsNullOrEmpty(info.account_id))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Account ID is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(info.new_password))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "New Password is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(info.confirm_password))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Confirm Password is required.";
                    return resp;
                }
                if (info.new_password.Length < 6)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "New Password is require minimum 6 digit.";
                    return resp;
                }
                if (info.confirm_password.Length < 6)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Confirm Password is require minimum 6 digit.";
                    return resp;
                }
                if (info.new_password != info.confirm_password)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Password doesn't match.";
                    return resp;
                }

                resp = _IAuthRepository.ChangePassword(info);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Change Password Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public MessageResponse ForgotPassword(ForgotPasswordModel email)
        {
            MessageResponse resp = new MessageResponse();
            try
            {
                int rest = _IAuthRepository.ForgotPassword(email.email);

                switch (rest)
                {
                    case 0:
                        // Send your email


                        //----------------
                        resp.status = StatusResponse.Success;
                        resp.message = "Please check link for reset password in your email.";
                        break;
                    case -1:
                        resp.message = "Please check your email.";
                        break;
                    case -2:
                        resp.message = "The account is not found.";
                        break;
                    default:
                        break;
                }

                _ILogRepository.WriteLogs("Forgot", email.email, "", "Success.");

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Forgot Password Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public MessageResponse SignUp(AccountModel account)
        {
            MessageResponse resp = new MessageResponse();
            try
            {
                int rest = _IAuthRepository.SignUp(account);

                switch (rest)
                {
                    case 0:
                        // Send your email


                        //----------------
                        resp.status = StatusResponse.Success;
                        resp.message = "You must activate your account with the code sent to your email address.";
                        break;
                    case -1:
                        resp.message = "Registration is not complete. Please check the information.";
                        break;
                    case -2:
                        resp.message = "You already have an account in the system.";
                        break;
                    default:
                        break;
                }

                _ILogRepository.WriteLogs("Sign Up", account.email, "", "Success.");

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignUp Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<RegisterModel> ClaimRegister(string token)
        {
            IList<RegisterModel> resp = new List<RegisterModel>();
            try
            {
                resp = _IAuthRepository.ClaimRegister(token);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRegister OwnerRegistered(RegisterModel data)
        {
            ResponseRegister resp = new ResponseRegister();
            try
            {
                var rest = _IAuthRepository.OwnerRegistered(data);
                resp.status = !string.IsNullOrEmpty(rest) ? StatusResponse.Success : StatusResponse.Error;
                resp.message = !string.IsNullOrEmpty(rest) ? "Register Successfull." : "Register Fail.";
                resp.token = rest;

                _ILogRepository.WriteLogs("Owner Register", data.email, rest, "Success.");

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Owner Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}