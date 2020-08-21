using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.PermissionGroup;

namespace Core.Api.Net.Business.Services.PermissionGroup
{
    public class PermissionGroupService : IPermissionGroupService

    {
        private readonly ILogs _ILogs;
        private readonly IPermissionGroupRepository _IPermissionGroupRepository;
        public PermissionGroupService(ILogs ILogs, IPermissionGroupRepository IPermissionGroupRepository)
        {
            _ILogs = ILogs;
            _IPermissionGroupRepository = IPermissionGroupRepository;
        }
        public IList<PermissionGroupModel> GetListPermissionGroup(string group_code)
        {
            IList<PermissionGroupModel> resp = new List<PermissionGroupModel>();
            try
            {
                resp = _IPermissionGroupRepository.GetListPermissionGroup(group_code);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionGroup Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponsePermissionGroup Update_PermissionGroup(string group_code, IList<PermissionGroupModel> data)
        {
            ResponsePermissionGroup resp = new ResponsePermissionGroup();
            try
            {
                var rest = _IPermissionGroupRepository.Update_PermissionGroup(group_code, data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionGroup Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }


    }
}