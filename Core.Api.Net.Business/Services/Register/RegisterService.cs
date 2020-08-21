using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.Register;

namespace Core.Api.Net.Business.Services.Register
{
    public class RegisterService : IRegisterService

    {
        private readonly ILogs _ILogs;
        private readonly IRegisterRepository _IRegisterRepository;
        public RegisterService(ILogs ILogs, IRegisterRepository IRegisterRepository)
        {
            _ILogs = ILogs;
            _IRegisterRepository = IRegisterRepository;
        }

        public IList<RegisterModel> GetListRegister()
        {
            IList<RegisterModel> resp = new List<RegisterModel>();
            try
            {
                resp = _IRegisterRepository.GetListRegister();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Get List Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRegister ActivateRegistered(RegisterModel data)
        {
            ResponseRegister resp = new ResponseRegister();
            try
            {
                int rest = _IRegisterRepository.ActivateRegistered(data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Activate Successfull." : "Activate Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Activate Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRegister Insert_Registered(RegisterModel data)
        {
            ResponseRegister resp = new ResponseRegister();
            try
            {
                int rest = _IRegisterRepository.Insert_Registered(data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";

                var getId = _IRegisterRepository.GetRegister(data.system_id);
                if (getId != null)
                {
                    resp.register_id = getId.register_id.ToString();
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRegister Update_Registered(RegisterModel data)
        {
            ResponseRegister resp = new ResponseRegister();
            try
            {
                int rest = _IRegisterRepository.Update_Registered(data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseRegister Delete_Registered(RegisterModel data)
        {
            ResponseRegister resp = new ResponseRegister();
            try
            {
                int rest = _IRegisterRepository.Delete_Registered(data);
                resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete Register Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }


    }
}