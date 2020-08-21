using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.TokenKey
{
    public interface ITokenKeyService
    {
        IList<TokenKeyModel> GetListTokenKey();
        ResponseTokenKey Insert_TokenKey(TokenKeyModel data);
        ResponseTokenKey Update_TokenKey(TokenKeyModel data);
        ResponseTokenKey Delete_TokenKey(TokenKeyModel data);
    }
}