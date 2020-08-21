using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.FuncMenu
{
    public interface IFuncMenuService
    {
        IList<FunctionModel> GetListFunction();
        ResponseFunction Insert_Function(FunctionModel data);
        ResponseFunction Update_Function(FunctionModel data);
        ResponseFunction Delete_Function(FunctionModel data);
    }
}