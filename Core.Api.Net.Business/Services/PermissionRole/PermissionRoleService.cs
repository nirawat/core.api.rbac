using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.PermissionRole;

namespace Core.Api.Net.Business.Services.PermissionRole
{
    public class PermissionRoleService : IPermissionRoleService

    {
        private readonly ILogs _ILogs;
        private readonly IPermissionRoleRepository _IPermissionRoleRepository;
        public PermissionRoleService(ILogs ILogs, IPermissionRoleRepository IPermissionRoleRepository)
        {
            _ILogs = ILogs;
            _IPermissionRoleRepository = IPermissionRoleRepository;
        }
        public IList<PermissionRoleModel> GetListPermissionRole(string group_code)
        {
            IList<PermissionRoleModel> resp = new List<PermissionRoleModel>();
            try
            {
                resp = _IPermissionRoleRepository.GetListPermissionRole(group_code);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionRole Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponsePermissionRole Update_PermissionRole(string group_code, IList<PermissionRoleModel> data)
        {
            ResponsePermissionRole resp = new ResponsePermissionRole();
            try
            {
                var rest = _IPermissionRoleRepository.Update_PermissionRole(group_code, data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionRole Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }


    }
}