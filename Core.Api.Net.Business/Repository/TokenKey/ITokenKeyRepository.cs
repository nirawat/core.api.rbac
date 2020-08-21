using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.TokenKey
{
    public interface ITokenKeyRepository
    {
        IList<TokenKeyModel> GetListTokenKey();
        int Insert_TokenKey(TokenKeyModel data);
        int Update_TokenKey(TokenKeyModel data);
        int Delete_TokenKey(TokenKeyModel data);

    }
}