using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.PermissionRole
{
    public interface IPermissionRoleService
    {
        IList<PermissionRoleModel> GetListPermissionRole(string group_code);
        ResponsePermissionRole Update_PermissionRole(string group_code, IList<PermissionRoleModel> data);
    }
}