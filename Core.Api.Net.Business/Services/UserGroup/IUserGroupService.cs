using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.UserGroup
{
    public interface IUserGroupService
    {
        IList<UserGroupModel> GetListUserGroup();
        ResponseUserGroup Insert_UserGroup(UserGroupModel data);
        ResponseUserGroup Update_UserGroup(UserGroupModel data);
        ResponseUserGroup Delete_UserGroup(UserGroupModel data);
    }
}