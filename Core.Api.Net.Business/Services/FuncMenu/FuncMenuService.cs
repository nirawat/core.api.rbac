using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.FuncMenu;

namespace Core.Api.Net.Business.Services.FuncMenu
{
    public class FuncMenuService : IFuncMenuService

    {
        private readonly ILogs _ILogs;
        private readonly IFuncMenuRepository _IFuncMenuRepository;
        public FuncMenuService(ILogs ILogs, IFuncMenuRepository IFuncMenuRepository)
        {
            _ILogs = ILogs;
            _IFuncMenuRepository = IFuncMenuRepository;
        }
        public IList<FunctionModel> GetListFunction()
        {
            IList<FunctionModel> resp = new List<FunctionModel>();
            try
            {
                resp = _IFuncMenuRepository.GetListFunction();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Function Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseFunction Validate_MasterFunction(FunctionModel data)
        {
            ResponseFunction resp = new ResponseFunction();
            try
            {
                if (string.IsNullOrEmpty(data.code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_thai))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name Thai is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.name_eng))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Name English is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.route_path))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Route Path is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.icon_name))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Icon Name is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.func_type))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Function Type is required.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate Function Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseFunction Insert_Function(FunctionModel data)
        {
            ResponseFunction resp = new ResponseFunction();
            try
            {
                var _validate = Validate_MasterFunction(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IFuncMenuRepository.Insert_Function(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Function Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseFunction Update_Function(FunctionModel data)
        {
            ResponseFunction resp = new ResponseFunction();
            try
            {
                var _validate = Validate_MasterFunction(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IFuncMenuRepository.Update_Function(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Function Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseFunction Delete_Function(FunctionModel data)
        {
            ResponseFunction resp = new ResponseFunction();
            try
            {
                var _validate = Validate_MasterFunction(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _IFuncMenuRepository.Delete_Function(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Function Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}