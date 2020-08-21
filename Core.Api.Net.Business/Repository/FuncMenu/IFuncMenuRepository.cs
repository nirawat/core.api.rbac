using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.FuncMenu
{
    public interface IFuncMenuRepository
    {
        FuncListModel GetFuncMenu(string email, string passw);
        IList<FunctionModel> GetListFunction();
        int Insert_Function(FunctionModel data);
        int Update_Function(FunctionModel data);
        int Delete_Function(FunctionModel data);
    }
}