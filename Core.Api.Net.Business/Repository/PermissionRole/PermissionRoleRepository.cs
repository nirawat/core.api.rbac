using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.PermissionRole
{
    public class PermissionRoleRepository : IPermissionRoleRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public PermissionRoleRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public IList<PermissionRoleModel> GetListPermissionRole(string group_code)
        {
            IList<PermissionRoleModel> resp = new List<PermissionRoleModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string role_code = "";
                    int seq = 0;

                    string queryGroup = string.Format(
                            @"SELECT C.code as group_code, C.name_thai as group_name,
                            A.code as role_code, A.name_thai as role_name,
                            (CASE WHEN C.code IS NULL THEN 0 ELSE 1 END) as used
                            FROM [dbo].[SYS_ROLE] A
                            LEFT OUTER JOIN [dbo].[SYS_PERMISSION_ROLE] B
                            ON A.code = B.role_code
                            LEFT OUTER JOIN [dbo].[SYS_USER_GROUP] C
                            ON B.group_code = C.code
                            WHERE C.code='{0}' ",
                            group_code);

                    var dtGroup = mssql.GetDataTableFromQueryStr(queryGroup);

                    if (dtGroup.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtGroup.Rows)
                        {
                            PermissionRoleModel permiss = new PermissionRoleModel()
                            {
                                seq = seq,
                                group_code = dr["group_code"].ToString(),
                                group_name = dr["group_name"].ToString(),
                                role_code = dr["role_code"].ToString(),
                                role_name = dr["role_name"].ToString(),
                                used = Convert.ToBoolean(dr["used"]),
                            };
                            role_code += string.Format(@"{0}{1}", dr["role_code"].ToString(), ",");
                            seq += 1;
                            resp.Add(permiss);
                        }

                    }


                    string role_code_array = role_code.Replace(",", "','");

                    string queryUsers = string.Format(
                            @"SELECT NULL as group_code, NULL as group_name, 
                            code as role_code, name_thai as role_name,0 as used
                            FROM [dbo].[SYS_ROLE]
                            WHERE 1=1 {0}",
                            role_code_array.Length > 0 ? " AND code NOT IN ('" + role_code_array.Remove(role_code_array.Length - 3, 3) + "')" : "");

                    var dtUser = mssql.GetDataTableFromQueryStr(queryUsers);

                    if (dtUser.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtUser.Rows)
                        {
                            PermissionRoleModel permiss = new PermissionRoleModel()
                            {
                                seq = seq,
                                group_code = dr["group_code"].ToString(),
                                group_name = dr["group_name"].ToString(),
                                role_code = dr["role_code"].ToString(),
                                role_name = dr["role_name"].ToString(),
                                used = Convert.ToBoolean(dr["used"]),
                            };
                            seq += 1;
                            resp.Add(permiss);
                        }

                    }

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList PermissionRole Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Update_PermissionRole(string group_code, IList<PermissionRoleModel> data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string queryDelete = string.Format("DELETE FROM SYS_PERMISSION_ROLE WHERE group_code='{0}'", group_code);

                    int restDelete = mssql.ExcuteNonQueryStr(queryDelete);

                    if (restDelete >= 0)
                    {
                        if (restDelete >= 0) { resp = 0; }

                        if (data != null && data.Count > 0)
                        {
                            foreach (var item in data)
                            {
                                string queryInsert = string.Format(
                                    @"INSERT INTO SYS_PERMISSION_ROLE 
                                    (group_code, role_code, create_date) 
                                    VALUES ('{0}','{1}','{2}')",
                                    group_code, item.role_code, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                int restInsert = mssql.ExcuteNonQueryStr(queryInsert);

                                if (restInsert >= 0) { resp = 0; }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert PermissionRole Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }


    }
}