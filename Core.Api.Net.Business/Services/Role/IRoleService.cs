using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.Role
{
    public interface IRoleService
    {
        IList<RoleModel> GetListRole();
        RoleModel GetRole(string code);
        ResponseRole Insert_Role(RoleModel data);
        ResponseRole Update_Role(RoleModel data);
        ResponseRole Delete_Role(RoleModel data);
    }
}