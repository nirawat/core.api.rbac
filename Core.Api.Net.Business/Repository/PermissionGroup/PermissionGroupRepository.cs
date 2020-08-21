using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.PermissionGroup
{
    public class PermissionGroupRepository : IPermissionGroupRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public PermissionGroupRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }


        public IList<PermissionGroupModel> GetListPermissionGroup(string group_code)
        {
            IList<PermissionGroupModel> resp = new List<PermissionGroupModel>();
            try
            {


                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string used_id = "";
                    int seq = 0;

                    string queryGroup = string.Format(
                            @"SELECT C.code as group_code, C.name_thai as group_name, A.account_id, 
                            (A.prefixes + A.first_name + ' ' + A.last_name) as account_full_name,
                            D.name_thai as account_department, (CASE WHEN C.code IS NULL THEN 0 ELSE 1 END) as used
                            FROM [dbo].[SYS_ACCOUNT] A
                            LEFT OUTER JOIN [dbo].[SYS_PERMISSION_GROUP] B
                            ON A.account_id = B.account_id
                            LEFT OUTER JOIN [dbo].[SYS_USER_GROUP] C
                            ON B.group_code = C.code
                            LEFT OUTER JOIN [dbo].[SYS_SECTION] D
                            ON A.section_code = D.code
                            WHERE C.code='{0}' ",
                            group_code);

                    var dtGroup = mssql.GetDataTableFromQueryStr(queryGroup);

                    if (dtGroup.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtGroup.Rows)
                        {
                            PermissionGroupModel permiss = new PermissionGroupModel()
                            {
                                seq = seq,
                                group_code = dr["group_code"].ToString(),
                                group_name = dr["group_name"].ToString(),
                                account_id = dr["account_id"].ToString(),
                                account_full_name = dr["account_full_name"].ToString(),
                                account_department = dr["account_department"].ToString(),
                                used = Convert.ToBoolean(dr["used"]),
                            };
                            used_id += string.Format(@"{0}{1}", dr["account_id"].ToString(), ",");
                            seq += 1;
                            resp.Add(permiss);
                        }

                    }


                    string account_id_array = used_id.Replace(",", "','");

                    string queryUsers = string.Format(
                            @"SELECT NULL as code, NULL as name_thai, A.account_id, 
                            (A.prefixes + A.first_name + ' ' + A.last_name) as account_full_name,
                            B.name_thai as account_department, 0 as used
                            FROM [dbo].[SYS_ACCOUNT] A
                            INNER JOIN [dbo].[SYS_SECTION] B
                            ON A.section_code = B.code
                            WHERE 1=1 {0}",
                            account_id_array.Length > 0 ? " AND A.account_id NOT IN ('" + account_id_array.Remove(account_id_array.Length - 3, 3) + "')" : "");

                    var dtUser = mssql.GetDataTableFromQueryStr(queryUsers);

                    if (dtUser.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtUser.Rows)
                        {
                            PermissionGroupModel permiss = new PermissionGroupModel()
                            {
                                seq = seq,
                                group_code = group_code,
                                group_name = "",
                                account_id = dr["account_id"].ToString(),
                                account_full_name = dr["account_full_name"].ToString(),
                                account_department = dr["account_department"].ToString(),
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
                _ILogs.LogError("GetList PermissionGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Update_PermissionGroup(string group_code, IList<PermissionGroupModel> data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string queryDelete = string.Format("DELETE FROM SYS_PERMISSION_GROUP WHERE group_code='{0}'", group_code);

                    int restDelete = mssql.ExcuteNonQueryStr(queryDelete);

                    if (restDelete >= 0)
                    {
                        if (restDelete >= 0) { resp = 0; }

                        if (data != null && data.Count > 0)
                        {
                            foreach (var item in data)
                            {
                                string queryInsert = string.Format(
                                    @"INSERT INTO SYS_PERMISSION_GROUP 
                                    (group_code, account_id, create_date) 
                                    VALUES ('{0}','{1}','{2}')",
                                    group_code, item.account_id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                int restInsert = mssql.ExcuteNonQueryStr(queryInsert);

                                if (restInsert >= 0) { resp = 0; }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert PermissionGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }


    }
}