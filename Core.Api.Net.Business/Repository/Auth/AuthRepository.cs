using System;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Models.Account;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Repository.FuncMenu;
using Newtonsoft.Json;
using System.Linq;
using Core.Api.Net.Business.Repository.Owner;
using Core.Api.Net.Business.Repository.Register;
using Core.Api.Net.Business.Repository.Logs;

namespace Core.Api.Net.Business.Repository.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private readonly IFuncMenuRepository _IFuncMenuRepository;
        private readonly IOwnerRepository _IOwnerRepository;
        private readonly IRegisterRepository _IRegisterRepository;
        private EnvironmentModel _EnvironmentModel;
        private readonly ILogRepository _ILogRepository;
        private readonly IJwtConfigs _JwtConfigs;
        public AuthRepository(ILogs ILogs,
        IHttpContextAccessor IHttpContextAccessor,
        IJwtConfigs JwtConfigs,
        IEnvironmentConfigs IEnvironmentConfigs,
        IFuncMenuRepository IFuncMenuRepository,
        IOwnerRepository IOwnerRepository,
        IRegisterRepository IRegisterRepository,
        ILogRepository ILogRepository
        )
        {
            _ILogs = ILogs;
            _IHttpContextAccessor = IHttpContextAccessor;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
            _JwtConfigs = JwtConfigs;
            _IFuncMenuRepository = IFuncMenuRepository;
            _IOwnerRepository = IOwnerRepository;
            _IRegisterRepository = IRegisterRepository;
            _ILogRepository = ILogRepository;
        }

        public SignInResponse SignIn(SignInModel userInfo)
        {
            SignInResponse resp = new SignInResponse();
            try
            {
                string passw = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(userInfo.password));

                string query = string.Format(
                    @"SELECT A.*, E.company_name_thai,E.company_name_eng,
                    C.name_thai AS group_name, C.theme_code
                    FROM [dbo].[SYS_ACCOUNT] A
                    INNER JOIN [dbo].[SYS_PERMISSION_GROUP] B
                    ON A.account_id = B.account_id
                    INNER JOIN [dbo].[SYS_USER_GROUP] C
                    ON B.group_code=C.code
                    LEFT OUTER JOIN [dbo].[SYS_COMPANY] E
                    ON A.company_id=E.company_id
                    WHERE A.email='{0}' AND A.password='{1}'",
                    userInfo.email, passw);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        resp.status = StatusResponse.Success;
                        resp.message = "success";

                        resp.userInfo = new UserInfoResponse();
                        foreach (DataRow dr in dt.Rows)
                        {
                            resp.userInfo.groupname = dr["group_name"].ToString();
                            resp.userInfo.accountId = dr["account_id"].ToString();
                            resp.userInfo.fullName = string.Format("{0}{1} {2}", dr["prefixes"].ToString(), dr["first_name"].ToString(), dr["last_name"].ToString());
                            resp.userInfo.email = dr["email"].ToString();
                            resp.userInfo.language = dr["language"].ToString();
                            resp.userInfo.theme = dr["theme_code"].ToString();
                            resp.userInfo.companyName = dr["company_name_eng"].ToString();
                        }
                        resp.userInfo.funcList = _IFuncMenuRepository.GetFuncMenu(userInfo.email, passw);
                        resp.userInfo.owner = _IOwnerRepository.GetOwner();
                        resp.userInfo.register = _IRegisterRepository.GetRegister(_EnvironmentModel.AppCodes.JwtCode);
                    }
                    else
                    {
                        resp.message = "The account is not found.";
                    }

                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignIn Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public SignInResponse SignOut(UserInfoResponse userInfo)
        {
            SignInResponse resp = new SignInResponse();
            try
            {
                string query = string.Format(
                    @"SELECT * FROM SYS_ACCOUNT WHERE email='{0}'", userInfo.email);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        resp.status = StatusResponse.Success;
                        resp.message = "success";

                        _ILogRepository.WriteLogs("Sign Out", userInfo.email, "", "Success.");
                    }

                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignOut Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int UpdateAccountStatus(string request_type, string account_id, string token, DateTime expire_date)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string update = string.Format(@"UPDATE SYS_ACCOUNT SET online_expire='{1}' WHERE account_id='{0}'",
                                                    account_id, expire_date.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(update);

                    if (rest >= 0)
                    {
                        _ILogRepository.WriteLogs(request_type, account_id, token, "Success.");
                        resp = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Account Status Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int ForgotPassword(string email)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string check_email = string.Format("SELECT * FROM SYS_ACCOUNT WHERE email='{0}'", email);

                    var dt = mssql.GetDataTableFromQueryStr(check_email);

                    if (dt.Rows.Count > 0)
                    {
                        string query = string.Format("INSERT INTO ACC_FORGOT_PASSWORD ([sent_to_email],[create_date]) VALUES ('{0}','{1}')", email, DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

                        int rest = mssql.ExcuteNonQueryStr(query);

                        if (rest >= 0) { resp = 0; }
                    }
                    else { resp = -2; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Forgot Password Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public ChangePasswordResponse ChangePassword(ChangePasswordModel info)
        {
            ChangePasswordResponse resp = new ChangePasswordResponse();
            try
            {
                string new_passw = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(info.new_password));

                string query = string.Format(@"SELECT * FROM SYS_ACCOUNT WHERE account_id='{0}'", info.account_id);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        string update = string.Format(@"UPDATE SYS_ACCOUNT SET password='{1}' WHERE account_id='{0}'", info.account_id, new_passw);

                        int rest = mssql.ExcuteNonQueryStr(update);

                        if (rest >= 0)
                        {
                            resp.status = StatusResponse.Success;
                            resp.message = "success.";
                            _ILogRepository.WriteLogs("Change Password", info.email, "", "Success.");
                        }
                    }
                    else
                    {
                        resp.status = StatusResponse.Error;
                        resp.message = "user not found.";
                    }

                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Change Password Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int SignUp(AccountModel account)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string check_email = string.Format("SELECT * FROM SYS_ACCOUNT WHERE email='{0}'", account.email);

                    var dt = mssql.GetDataTableFromQueryStr(check_email);

                    if (dt.Rows.Count > 0) { resp = -2; }
                    else
                    {
                        string password = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(account.password));

                        string query = string.Format("INSERT INTO SYS_ACCOUNT ([email],[password],[first_name],[last_name]) VALUES ('{0}','{1}','{2}','{3}')",
                                                      account.email, password, account.first_name, account.last_name);

                        int rest = mssql.ExcuteNonQueryStr(query);

                        if (rest >= 0) { resp = 0; }
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("SignUp Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public IList<RegisterModel> ClaimRegister(string token)
        {
            IList<RegisterModel> resp = new List<RegisterModel>();
            try
            {
                RegisterModel _claim = ReadRegisterToken(token);

                resp.Add(_claim);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Get Register Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public string OwnerRegistered(RegisterModel data)
        {
            try
            {
                string token = BuildRegisterToken(data);

                return token;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Owner Registered Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return string.Empty;
        }

        public string BuildToken(string request_type, string account_id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(_JwtConfigs.Secret)));

            DateTime _online_expire = DateTime.UtcNow.AddMinutes(_JwtConfigs.Expires);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(), ClaimValueTypes.Integer64),
                }),
                Issuer = _JwtConfigs.Issuer,
                Audience = _JwtConfigs.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = _online_expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            string _token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            //Update Account Status
            UpdateAccountStatus(request_type, account_id, _token, _online_expire);

            return _token;
        }

        public string ReadToken()
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = _IHttpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                authHeader = authHeader.Replace("Bearer ", "");
                //var jsonToken = handler.ReadToken(authHeader);
                //var tokenS = handler.ReadToken(authHeader) as JwtSecurityToken;
                //string UserData = tokenS.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/userdata").Value;
                //resp = JsonConvert.DeserializeObject<DataTokenRespones>(UserData);
            }
            return authHeader;
        }

        public string BuildRegisterToken(RegisterModel register)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Encoding.UTF8.GetString(Convert.FromBase64String(_JwtConfigs.Secret)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(), ClaimValueTypes.Integer64),
                    new Claim(JwtRegisteredClaimNames.FamilyName, JsonConvert.SerializeObject(register)),
                }),
                Issuer = _JwtConfigs.Issuer,
                Audience = _JwtConfigs.Audience,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(_JwtConfigs.ExpiresRegister),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public RegisterModel ReadRegisterToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            RegisterModel resp = new RegisterModel();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string _family_name = tokenS.Claims.First(claim => claim.Type == "family_name").Value;
                resp = JsonConvert.DeserializeObject<RegisterModel>(_family_name);
            }
            return resp;
        }
    }
}