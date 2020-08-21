using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.User
{
    public interface IUserRepository
    {
        IList<UserModel> GetListUser();
        UserModel GetUser(string accountId, string email);
        int Insert_User(UserModel data);
        int Update_User(UserModel data);
        int Delete_User(UserModel data);

    }
}