using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.PermissionRole
{
    public interface IPermissionRoleRepository
    {
        IList<PermissionRoleModel> GetListPermissionRole(string group_code);
        int Update_PermissionRole(string group_code, IList<PermissionRoleModel> data);

    }
}