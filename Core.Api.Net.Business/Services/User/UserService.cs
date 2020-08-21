using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.User;

namespace Core.Api.Net.Business.Services.User
{
    public class UserService : IUserService

    {
        private readonly ILogs _ILogs;
        private readonly IUserRepository _IUserRepository;
        public UserService(ILogs ILogs, IUserRepository IUserRepository)
        {
            _ILogs = ILogs;
            _IUserRepository = IUserRepository;
        }

        public UserModel GetUser(string accountId, string email)
        {
            UserModel resp = new UserModel();
            try
            {
                resp = _IUserRepository.GetUser(accountId, email);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<UserModel> GetListUser()
        {
            IList<UserModel> resp = new List<UserModel>();
            try
            {
                resp = _IUserRepository.GetListUser();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUser Validate_MasterUser(UserModel data)
        {
            ResponseUser resp = new ResponseUser();
            try
            {
                if (string.IsNullOrEmpty(data.email))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Email is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.password))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Password is required.";
                    return resp;
                }
                // if (string.IsNullOrEmpty(data.prefixes))
                // {
                //     resp.status = StatusResponse.Error;
                //     resp.message = "prefixes is required.";
                //     return resp;
                // }
                if (string.IsNullOrEmpty(data.first_name))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "First Name is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.section_code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Section is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.language))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Language is required.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate User Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUser Insert_User(UserModel data)
        {
            ResponseUser resp = new ResponseUser();
            try
            {
                var _validate = Validate_MasterUser(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getUser = _IUserRepository.GetUser("", data.email);
                    if (getUser != null)
                    {
                        resp.status = StatusResponse.Error;
                        resp.message = "Email is duplicate.";
                        return resp;
                    }

                    var rest = _IUserRepository.Insert_User(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";

                    var getId = _IUserRepository.GetUser("", data.email);
                    if (getId != null)
                    {
                        resp.account_id = getId.account_id.ToString();
                    }
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUser Update_User(UserModel data)
        {
            ResponseUser resp = new ResponseUser();
            try
            {
                var _validate = Validate_MasterUser(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getUser = _IUserRepository.GetUser(data.account_id, "");

                    if (getUser != null)
                    {
                        if (getUser.email != data.email)
                        {
                            resp.status = StatusResponse.Error;
                            resp.message = "Email is duplicate.";
                            return resp;
                        }
                    }

                    var rest = _IUserRepository.Update_User(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseUser Delete_User(UserModel data)
        {
            ResponseUser resp = new ResponseUser();
            try
            {
                var _validate = Validate_MasterUser(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IUserRepository.Delete_User(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}