using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.PermissionFunc
{
    public interface IPermissionFuncRepository
    {
        IList<PermissionFuncModel> GetListPermissionFunc(string group_code);
        int Update_PermissionFunc(string group_code, IList<PermissionFuncModel> data);

    }
}