using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.PermissionFunc;

namespace Core.Api.Net.Business.Services.PermissionFunc
{
    public class PermissionFuncService : IPermissionFuncService

    {
        private readonly ILogs _ILogs;
        private readonly IPermissionFuncRepository _IPermissionFuncRepository;
        public PermissionFuncService(ILogs ILogs, IPermissionFuncRepository IPermissionFuncRepository)
        {
            _ILogs = ILogs;
            _IPermissionFuncRepository = IPermissionFuncRepository;
        }
        public IList<PermissionFuncModel> GetListPermissionFunc(string group_code)
        {
            IList<PermissionFuncModel> resp = new List<PermissionFuncModel>();
            try
            {
                resp = _IPermissionFuncRepository.GetListPermissionFunc(group_code);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionFunc Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponsePermissionFunc Update_PermissionFunc(string group_code, IList<PermissionFuncModel> data)
        {
            ResponsePermissionFunc resp = new ResponsePermissionFunc();
            try
            {
                var rest = _IPermissionFuncRepository.Update_PermissionFunc(group_code, data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("PermissionFunc Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }


    }
}