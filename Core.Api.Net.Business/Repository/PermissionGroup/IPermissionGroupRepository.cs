using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.PermissionGroup
{
    public interface IPermissionGroupRepository
    {
        IList<PermissionGroupModel> GetListPermissionGroup(string group_code);
        int Update_PermissionGroup(string group_code, IList<PermissionGroupModel> data);

    }
}