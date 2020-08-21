using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Register
{
    public interface IRegisterRepository
    {
        IList<RegisterModel> GetListRegister();
        RegisterModel GetRegister(string system_id);
        int ActivateRegistered(RegisterModel data);
        int Insert_Registered(RegisterModel data);
        int Update_Registered(RegisterModel data);
        int Delete_Registered(RegisterModel data);

    }
}