using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Role
{
    public interface IRoleRepository
    {
        IList<RoleModel> GetListRole();
        RoleModel GetRole(string code);
        int Insert_Role(RoleModel data);
        int Update_Role(RoleModel data);
        int Delete_Role(RoleModel data);

    }
}