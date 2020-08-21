using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.User
{
    public interface IUserService
    {
        IList<UserModel> GetListUser();
        UserModel GetUser(string accountId, string email);
        ResponseUser Insert_User(UserModel data);
        ResponseUser Update_User(UserModel data);
        ResponseUser Delete_User(UserModel data);
    }
}