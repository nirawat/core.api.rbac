using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public UserRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public UserModel GetUser(string accountId, string email)
        {
            UserModel resp = new UserModel();
            try
            {
                string query = "SELECT * FROM SYS_ACCOUNT WHERE 1=1 ";
                string condition = "";

                if (!string.IsNullOrEmpty(accountId))
                {
                    condition += string.Format(@" AND account_id='{0}'", accountId);
                }
                if (!string.IsNullOrEmpty(email))
                {
                    condition += string.Format(@" AND email='{0}'", email);
                }

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query + condition);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            resp.account_id = dr["account_id"].ToString();
                            resp.email = dr["email"].ToString();
                            resp.password = Encoding.UTF8.GetString(Convert.FromBase64String(dr["password"].ToString()));
                            resp.prefixes = dr["prefixes"].ToString();
                            resp.first_name = dr["first_name"].ToString();
                            resp.last_name = dr["last_name"].ToString();
                            resp.language = dr["language"].ToString();
                            resp.section_code = dr["section_code"].ToString();
                            resp.activate_date = DBNull.Value.Equals(dr["activate_datetime"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["activate_datetime"]);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Get User Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<UserModel> GetListUser()
        {
            IList<UserModel> resp = new List<UserModel>();
            try
            {
                string query = string.Format(
                        @"SELECT A.*, B.name_thai as section_name 
                        FROM SYS_ACCOUNT A
                        INNER JOIN SYS_SECTION B
                        ON A.section_code = B.code");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            UserModel token_model = new UserModel()
                            {
                                account_id = dr["account_id"].ToString(),
                                email = dr["email"].ToString(),
                                password = Encoding.UTF8.GetString(Convert.FromBase64String(dr["password"].ToString())),
                                prefixes = dr["prefixes"].ToString(),
                                first_name = dr["first_name"].ToString(),
                                last_name = dr["last_name"].ToString(),
                                language = dr["language"].ToString(),
                                section_code = dr["section_code"].ToString(),
                                section_name = dr["section_name"].ToString(),
                                activate_date = DBNull.Value.Equals(dr["activate_datetime"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["activate_datetime"]),
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList User Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Insert_User(UserModel data)
        {
            int resp = -1;
            try
            {
                string _password = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data.password));

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_ACCOUNT
                                                (email, password, prefixes, first_name, last_name, language, section_code, activate_datetime) 
                                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                data.email, _password, data.prefixes, data.first_name, data.last_name, data.language, data.section_code, data.activate_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert User Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_User(UserModel data)
        {
            int resp = -1;
            try
            {
                string _password = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data.password));

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format(@"UPDATE SYS_ACCOUNT SET 
                                                email='{1}', password='{2}', prefixes='{3}', first_name='{4}', last_name='{5}', language='{6}', section_code='{7}', activate_datetime='{8}'
                                                WHERE account_id='{0}'",
                                                data.account_id, data.email, _password, data.prefixes, data.first_name, data.last_name, data.language, data.section_code, data.activate_date.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update User Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_User(UserModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format("DELETE FROM SYS_ACCOUNT WHERE account_id='{0}'", data.account_id);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete User Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

    }
}