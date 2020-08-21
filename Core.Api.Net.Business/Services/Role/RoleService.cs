using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.Role;

namespace Core.Api.Net.Business.Services.Role
{
    public class RoleService : IRoleService

    {
        private readonly ILogs _ILogs;
        private readonly IRoleRepository _IRoleRepository;
        public RoleService(ILogs ILogs, IRoleRepository IRoleRepository)
        {
            _ILogs = ILogs;
            _IRoleRepository = IRoleRepository;
        }

        public RoleModel GetRole(string code)
        {
            RoleModel resp = new RoleModel();
            try
            {
                resp = _IRoleRepository.GetRole(code);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<RoleModel> GetListRole()
        {
            IList<RoleModel> resp = new List<RoleModel>();
            try
            {
                resp = _IRoleRepository.GetListRole();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRole Validate_MasterRole(RoleModel data)
        {
            ResponseRole resp = new ResponseRole();
            try
            {
                if (string.IsNullOrEmpty(data.code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required.";
                    return resp;
                }
                if (data.code.Length < 3)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required 3 digit.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_thai))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name (Thai) is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_eng))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name (Eng) is required.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRole Insert_Role(RoleModel data)
        {
            ResponseRole resp = new ResponseRole();
            try
            {
                var _validate = Validate_MasterRole(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getRole = _IRoleRepository.GetRole(data.code);
                    if (getRole != null)
                    {
                        resp.status = StatusResponse.Error;
                        resp.message = "Role Code is duplicate.";
                        return resp;
                    }

                    var rest = _IRoleRepository.Insert_Role(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRole Update_Role(RoleModel data)
        {
            ResponseRole resp = new ResponseRole();
            try
            {
                var _validate = Validate_MasterRole(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var getRole = _IRoleRepository.GetRole(data.code);

                    if (getRole != null)
                    {
                        if (getRole.code != data.code)
                        {
                            resp.status = StatusResponse.Error;
                            resp.message = "Role Code is duplicate.";
                            return resp;
                        }
                    }

                    var rest = _IRoleRepository.Update_Role(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRole Delete_Role(RoleModel data)
        {
            ResponseRole resp = new ResponseRole();
            try
            {
                var _validate = Validate_MasterRole(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IRoleRepository.Delete_Role(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Role Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}