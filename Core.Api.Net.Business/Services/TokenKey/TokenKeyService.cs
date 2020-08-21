using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Repository.TokenKey;

namespace Core.Api.Net.Business.Services.TokenKey
{
    public class TokenKeyService : ITokenKeyService

    {
        private readonly ILogs _ILogs;
        private readonly ITokenKeyRepository _ITokenKeyRepository;
        public TokenKeyService(ILogs ILogs, ITokenKeyRepository ITokenKeyRepository)
        {
            _ILogs = ILogs;
            _ITokenKeyRepository = ITokenKeyRepository;
        }
        public IList<TokenKeyModel> GetListTokenKey()
        {
            IList<TokenKeyModel> resp = new List<TokenKeyModel>();
            try
            {
                resp = _ITokenKeyRepository.GetListTokenKey();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Token Key Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseTokenKey Validate_MasterTokenKey(TokenKeyModel data)
        {
            ResponseTokenKey resp = new ResponseTokenKey();
            try
            {
                if (string.IsNullOrEmpty(data.code))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Code is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.secret))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Secret is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.issuer))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Issuer is required.";
                    return resp;
                }
                if (string.IsNullOrEmpty(data.audience))
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Audience is required.";
                    return resp;
                }
                if (data.expire <= 0)
                {
                    resp.status = StatusResponse.Error;
                    resp.message = "Expire is required more than Zero.";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Validate User Group Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseTokenKey Insert_TokenKey(TokenKeyModel data)
        {
            ResponseTokenKey resp = new ResponseTokenKey();
            try
            {
                var _validate = Validate_MasterTokenKey(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _ITokenKeyRepository.Insert_TokenKey(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Insert Successfull." : "Insert Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Token Key Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseTokenKey Update_TokenKey(TokenKeyModel data)
        {
            ResponseTokenKey resp = new ResponseTokenKey();
            try
            {
                var _validate = Validate_MasterTokenKey(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _ITokenKeyRepository.Update_TokenKey(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Update Successfull." : "Update Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Token Key Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public ResponseTokenKey Delete_TokenKey(TokenKeyModel data)
        {
            ResponseTokenKey resp = new ResponseTokenKey();
            try
            {
                var _validate = Validate_MasterTokenKey(data);

                if (_validate != null)
                {
                    resp = _validate;
                }
                else
                {
                    var rest = _ITokenKeyRepository.Delete_TokenKey(data);
                    resp.status = rest >= 0 ? StatusResponse.Success : StatusResponse.Error;
                    resp.message = rest >= 0 ? "Delete Successfull." : "Delete Fail.";
                }
                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Token Key Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}