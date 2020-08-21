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

namespace Core.Api.Net.Business.Repository.Role
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public RoleRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public RoleModel GetRole(string code)
        {
            RoleModel resp = new RoleModel();
            try
            {
                string query = string.Format(@"SELECT * FROM SYS_ROLE WHERE code='{0}'", code);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            resp.code = dr["code"].ToString();
                            resp.name_thai = dr["name_thai"].ToString();
                            resp.name_eng = dr["name_eng"].ToString();
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList Role Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<RoleModel> GetListRole()
        {
            IList<RoleModel> resp = new List<RoleModel>();
            try
            {
                string query = string.Format(@"SELECT * FROM SYS_ROLE");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            RoleModel token_model = new RoleModel()
                            {
                                code = dr["code"].ToString(),
                                name_thai = dr["name_thai"].ToString(),
                                name_eng = dr["name_eng"].ToString(),
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList Role Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Insert_Role(RoleModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_ROLE (code, name_thai, name_eng) VALUES ('{0}','{1}','{2}')",
                                                data.code, data.name_thai, data.name_eng);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert Role Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_Role(RoleModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format(@"UPDATE SYS_ROLE SET name_thai='{1}', name_eng='{2}' WHERE code='{0}'",
                                                data.code, data.name_thai, data.name_eng);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Role Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_Role(RoleModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format("DELETE FROM SYS_ROLE WHERE code='{0}'", data.code);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete Role Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

    }
}