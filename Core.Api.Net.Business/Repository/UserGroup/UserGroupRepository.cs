using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.UserGroup
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public UserGroupRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }


        public IList<UserGroupModel> GetListUserGroup()
        {
            IList<UserGroupModel> resp = new List<UserGroupModel>();
            try
            {
                string query = string.Format(
                        @"SELECT A.code, A.name_thai, A.name_eng, A.group_level, 
                        A.theme_code as theme_code_color, A.create_date,
                        ISNULL((SELECT  COUNT(group_code) FROM [dbo].[SYS_PERMISSION_GROUP] WHERE group_code=A.code GROUP BY group_code),0) as count_member,
                        ISNULL((SELECT  COUNT(group_code) FROM [dbo].[SYS_PERMISSION_ROLE] WHERE group_code=A.code GROUP BY group_code),0) as count_role,
                        ISNULL((SELECT  COUNT(group_code) FROM [dbo].[SYS_PERMISSION_FUNCTION] WHERE group_code=A.code GROUP BY group_code),0) as count_func
                        FROM [dbo].[SYS_USER_GROUP] A
                        GROUP BY A.code, A.name_thai, A.name_eng, A.group_level, 
                        A.theme_code, A.create_date");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            UserGroupModel group_model = new UserGroupModel()
                            {
                                code = dr["code"].ToString(),
                                name_thai = dr["name_thai"].ToString(),
                                name_eng = dr["name_eng"].ToString(),
                                group_level = Convert.ToInt32(dr["group_level"]),
                                theme_code = dr["theme_code_color"].ToString(),
                                count_member = Convert.ToInt32(dr["count_member"]),
                                count_role = Convert.ToInt32(dr["count_role"]),
                                count_func = Convert.ToInt32(dr["count_func"]),
                                create_date = Convert.ToDateTime(dr["create_date"]),
                            };
                            resp.Add(group_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList UserGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Insert_UserGroup(UserGroupModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_USER_GROUP 
                                                (code, name_thai, name_eng, group_level, theme_code, create_date) 
                                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                data.code, data.name_thai, data.name_eng, data.group_level, data.theme_code, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert UserGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_UserGroup(UserGroupModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format(@"UPDATE SYS_USER_GROUP SET 
                                                name_thai='{1}', name_eng='{2}', group_level='{3}', theme_code='{4}', create_date='{5}' 
                                                WHERE code='{0}'",
                                                data.code, data.name_thai, data.name_eng, data.group_level, data.theme_code, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update UserGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_UserGroup(UserGroupModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format("DELETE FROM SYS_USER_GROUP WHERE code='{0}'", data.code);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete UserGroup Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

    }
}