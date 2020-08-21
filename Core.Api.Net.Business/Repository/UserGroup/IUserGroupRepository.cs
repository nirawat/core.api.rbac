using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.UserGroup
{
    public interface IUserGroupRepository
    {
        IList<UserGroupModel> GetListUserGroup();
        int Insert_UserGroup(UserGroupModel data);
        int Update_UserGroup(UserGroupModel data);
        int Delete_UserGroup(UserGroupModel data);

    }
}