using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.PermissionFunc
{
    public interface IPermissionFuncService
    {
        IList<PermissionFuncModel> GetListPermissionFunc(string group_code);
        ResponsePermissionFunc Update_PermissionFunc(string group_code, IList<PermissionFuncModel> data);
    }
}