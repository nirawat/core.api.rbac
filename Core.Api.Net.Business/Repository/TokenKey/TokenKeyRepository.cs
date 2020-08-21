using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.TokenKey
{
    public class TokenKeyRepository : ITokenKeyRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public TokenKeyRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }


        public IList<TokenKeyModel> GetListTokenKey()
        {
            IList<TokenKeyModel> resp = new List<TokenKeyModel>();
            try
            {
                string query = string.Format(
                        @"SELECT * FROM SYS_JWT_TOKEN
                        ORDER BY create_date ASC");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            TokenKeyModel token_model = new TokenKeyModel()
                            {
                                code = dr["code"].ToString(),
                                secret = dr["secret"].ToString(),
                                issuer = dr["issuer"].ToString(),
                                audience = dr["audience"].ToString(),
                                expire = Convert.ToInt32(dr["expires"]),
                                create_date = Convert.ToDateTime(dr["create_date"]),
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList TokenKey Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Insert_TokenKey(TokenKeyModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_JWT_TOKEN 
                                                (code, secret, issuer, audience, expires, create_date) 
                                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                data.code, data.secret, data.issuer, data.audience, data.expire, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert TokenKey Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_TokenKey(TokenKeyModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format(@"UPDATE SYS_JWT_TOKEN SET 
                                                secret='{1}', issuer='{2}', audience='{3}', expires='{4}', create_date='{5}' 
                                                WHERE code='{0}'",
                                                data.code, data.secret, data.issuer, data.audience, data.expire, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update TokenKey Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_TokenKey(TokenKeyModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format("DELETE FROM SYS_JWT_TOKEN WHERE code='{0}'", data.code);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete TokenKey Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

    }
}