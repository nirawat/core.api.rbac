using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.Register
{
    public interface IRegisterService
    {
        IList<RegisterModel> GetListRegister();
        ResponseRegister ActivateRegistered(RegisterModel data);
        ResponseRegister Insert_Registered(RegisterModel data);
        ResponseRegister Update_Registered(RegisterModel data);
        ResponseRegister Delete_Registered(RegisterModel data);
    }
}