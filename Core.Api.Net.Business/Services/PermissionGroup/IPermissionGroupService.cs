using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.PermissionGroup
{
    public interface IPermissionGroupService
    {
        IList<PermissionGroupModel> GetListPermissionGroup(string group_code);
        ResponsePermissionGroup Update_PermissionGroup(string group_code, IList<PermissionGroupModel> data);
    }
}