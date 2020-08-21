using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Repository.FuncMenu;

namespace Core.Api.Net.Business.Repository.FuncMenu
{
    public class FuncMenuRepository : IFuncMenuRepository
    {

        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public FuncMenuRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public FuncListModel GetFuncMenu(string email, string passw)
        {
            FuncListModel resp = new FuncListModel();
            try
            {

                resp.func = new List<FuncModel>();

                string query = string.Format(
                        @"select C.seq,D.code, D.name_thai AS func_name, 
                        D.route_path, D.icon_name, D.func_type, D.func_ref_sub
                        from [dbo].[SYS_ACCOUNT] A
                        INNER JOIN [dbo].[SYS_PERMISSION_GROUP] B
                        ON A.account_id = B.account_id
                        INNER JOIN [dbo].[SYS_PERMISSION_FUNCTION] C
                        ON B.group_code = C.group_code
                        INNER JOIN [dbo].[SYS_FUNCTION] D
                        ON C.func_code = D.code
                        WHERE A.email='{0}'AND A.password='{1}'
                        ORDER BY b.group_code,c.seq ASC", email, passw);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string func_type = dr["func_type"].ToString();

                            if (func_type == "Main" || func_type == "MainSub")
                            {
                                FuncModel func_model = new FuncModel()
                                {
                                    seq = Convert.ToInt32(dr["seq"]),
                                    key = dr["code"].ToString(),
                                    icon = dr["icon_name"].ToString(),
                                    path = func_type == "MainSub" ? null : dr["route_path"].ToString(),
                                    sub_item = new List<SubFuncModel>(),
                                };
                                resp.func.Add(func_model);
                            }
                            if (func_type == "Divider")
                            {
                                FuncModel func_model = new FuncModel()
                                {
                                    seq = Convert.ToInt32(dr["seq"]),
                                    divider = true,
                                };
                                resp.func.Add(func_model);
                            }
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            string func_type = dr["func_type"].ToString();

                            if (func_type == "SubMenu")
                            {
                                SubFuncModel sub_item = new SubFuncModel()
                                {
                                    seq = Convert.ToInt32(dr["seq"]),
                                    key = dr["code"].ToString(),
                                    icon = dr["icon_name"].ToString(),
                                    path = dr["route_path"].ToString(),
                                };
                                int indexFunc = resp.func.FindIndex(e => e.key != null && e.key == dr["func_ref_sub"].ToString());
                                resp.func[indexFunc].sub_item.Add(sub_item);
                            }
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Function Application: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<FunctionModel> GetListFunction()
        {
            IList<FunctionModel> resp = new List<FunctionModel>();
            try
            {
                string query = string.Format(
                        @"SELECT * FROM SYS_FUNCTION");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            FunctionModel token_model = new FunctionModel()
                            {
                                code = dr["code"].ToString(),
                                name_thai = dr["name_thai"].ToString(),
                                name_eng = dr["name_eng"].ToString(),
                                route_path = dr["route_path"].ToString(),
                                icon_name = dr["icon_name"].ToString(),
                                func_type = dr["func_type"].ToString(),
                                func_ref_sub = dr["func_ref_sub"].ToString(),
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList Function Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int Insert_Function(FunctionModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_FUNCTION 
                                                (code, name_thai, name_eng, route_path, icon_name, func_type, func_ref_sub) 
                                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                                                data.code, data.name_thai, data.name_eng, data.route_path, data.icon_name, data.func_type, data.func_ref_sub);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert Function Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_Function(FunctionModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format(@"UPDATE SYS_FUNCTION SET 
                                                name_thai='{1}', name_eng='{2}', route_path='{3}', icon_name='{4}', func_type='{5}', func_ref_sub='{6}' 
                                                WHERE code='{0}'",
                                                data.code, data.name_thai, data.name_eng, data.route_path, data.icon_name, data.func_type, data.func_ref_sub);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Function Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_Function(FunctionModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {

                    string query = string.Format("DELETE FROM SYS_FUNCTION WHERE code='{0}'", data.code);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete Function Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }


    }
}