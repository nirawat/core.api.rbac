using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.PermissionFunc
{
    public class PermissionFuncRepository : IPermissionFuncRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public PermissionFuncRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public IList<PermissionFuncModel> GetListPermissionFunc(string group_code)
        {
            IList<PermissionFuncModel> resp = new List<PermissionFuncModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string func_code = "";
                    int seq = 0;

                    string queryGroup = string.Format(
                            @"SELECT C.code as group_code, C.name_thai as group_name,
                            A.code as func_code, A.name_thai as func_name,
                            A.func_type, A.func_ref_sub,
                            (CASE WHEN C.code IS NULL THEN 0 ELSE 1 END) as used
                            FROM [dbo].[SYS_FUNCTION] A
                            LEFT OUTER JOIN [dbo].[SYS_PERMISSION_FUNCTION] B
                            ON A.code = B.func_code
                            LEFT OUTER JOIN [dbo].[SYS_USER_GROUP] C
                            ON B.group_code = C.code
                            WHERE C.code='{0}' ",
                            group_code);

                    var dtGroup = mssql.GetDataTableFromQueryStr(queryGroup);

                    if (dtGroup.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtGroup.Rows)
                        {
                            PermissionFuncModel permiss = new PermissionFuncModel()
                            {
                                seq = seq,
                                group_code = dr["group_code"].ToString(),
                                group_name = dr["group_name"].ToString(),
                                func_code = dr["func_code"].ToString(),
                                func_name = dr["func_name"].ToString(),
                                func_type = dr["func_type"].ToString(),
                                func_ref_sub = dr["func_ref_sub"].ToString(),
                                used = Convert.ToBoolean(dr["used"]),
                            };
                            func_code += string.Format(@"{0}{1}", dr["func_code"].ToString(), ",");
                            seq += 1;
                            resp.Add(permiss);
                        }

                    }


                    string func_code_array = func_code.Replace(",", "','");

                    string queryUsers = string.Format(
                            @"SELECT NULL as group_code, NULL as group_name, 
                            code as func_code, name_thai as func_name,
                            func_type, func_ref_sub, 0 as used
                            FROM [dbo].[SYS_FUNCTION]
                            WHERE 1=1 {0}",
                            func_code_array.Length > 0 ? " AND code NOT IN ('" + func_code_array.Remove(func_code_array.Length - 3, 3) + "')" : "");

                    var dtUser = mssql.GetDataTableFromQueryStr(queryUsers);

                    if (dtUser.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtUser.Rows)
                        {
                            PermissionFuncModel permiss = new PermissionFuncModel()
                            {
                                seq = seq,
                                group_code = dr["group_code"].ToString(),
                                group_name = dr["group_name"].ToString(),
                                func_code = dr["func_code"].ToString(),
                                func_name = dr["func_name"].ToString(),
                                func_type = dr["func_type"].ToString(),
                                func_ref_sub = dr["func_ref_sub"].ToString(),
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
                _ILogs.LogError("GetList PermissionFunc Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Update_PermissionFunc(string group_code, IList<PermissionFuncModel> data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string queryDelete = string.Format("DELETE FROM SYS_PERMISSION_FUNCTION WHERE group_code='{0}'", group_code);

                    int restDelete = mssql.ExcuteNonQueryStr(queryDelete);

                    if (restDelete >= 0)
                    {
                        if (restDelete >= 0) { resp = 0; }

                        if (data != null && data.Count > 0)
                        {
                            int seq = 1;
                            foreach (var item in data)
                            {
                                string queryInsert = string.Format(
                                    @"INSERT INTO SYS_PERMISSION_FUNCTION 
                                    (group_code, func_code, seq, create_date) 
                                    VALUES ('{0}','{1}','{2}','{3}')",
                                    group_code, item.func_code, seq, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                                int restInsert = mssql.ExcuteNonQueryStr(queryInsert);

                                if (restInsert >= 0) { resp = 0; }
                                seq += 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert PermissionFunc Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }


    }
}