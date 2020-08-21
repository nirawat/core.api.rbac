using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Repository.Auth;
using Microsoft.AspNetCore.Http;

namespace Core.Api.Net.Business.Repository.Register
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public RegisterRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }
        public IList<RegisterModel> GetListRegister()
        {
            IList<RegisterModel> resp = new List<RegisterModel>();
            try
            {
                string query = string.Format(@"SELECT * FROM SYS_Register");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            RegisterModel token_model = new RegisterModel()
                            {
                                register_id = dr["register_id"].ToString(),
                                system_id = dr["system_id"].ToString(),
                                token = dr["token"].ToString(),
                                name_eng = dr["name_eng"].ToString(),
                                name_thai = dr["name_thai"].ToString(),
                                address = dr["address"].ToString(),
                                email = dr["email"].ToString(),
                                website = dr["website"].ToString(),
                                tel = dr["tel"].ToString(),
                                fax = dr["fax"].ToString(),
                                contact = dr["contact"].ToString(),
                                country = dr["country"].ToString(),
                                register_date = Convert.ToDateTime(dr["register_date"]),
                                activate_date = DBNull.Value.Equals(dr["activate_date"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["activate_date"]),
                                expire_date = DBNull.Value.Equals(dr["expire_date"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["expire_date"]),
                                activate = DBNull.Value.Equals(dr["activate_date"]) ? false : true,
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("GetList Register Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public RegisterModel GetRegister(string system_id)
        {
            RegisterModel resp = new RegisterModel();
            try
            {
                string query = string.Format(@"SELECT * FROM SYS_REGISTER WHERE system_id='{0}'", system_id);

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            resp.register_id = dr["register_id"].ToString();
                            resp.system_id = dr["system_id"].ToString();
                            resp.token = dr["token"].ToString();
                            resp.name_eng = dr["name_eng"].ToString();
                            resp.name_thai = dr["name_thai"].ToString();
                            resp.address = dr["address"].ToString();
                            resp.email = dr["email"].ToString();
                            resp.website = dr["website"].ToString();
                            resp.tel = dr["tel"].ToString();
                            resp.fax = dr["fax"].ToString();
                            resp.contact = dr["contact"].ToString();
                            resp.country = dr["country"].ToString();
                            resp.register_date = Convert.ToDateTime(dr["register_date"]);
                            resp.activate_date = DBNull.Value.Equals(dr["activate_date"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["activate_date"]);
                            resp.expire_date = DBNull.Value.Equals(dr["expire_date"]) ? Convert.ToDateTime("01/01/1999") : Convert.ToDateTime(dr["expire_date"]);
                            resp.activate = DBNull.Value.Equals(dr["activate_date"]) ? false : true;
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Get Register Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public int ActivateRegistered(RegisterModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(
                                    @"UPDATE SYS_REGISTER SET
                                    [token]='{1}',[activate_date]='{2}',[expire_date]='{3}'
                                    WHERE register_id='{0}'",
                                    data.register_id, data.token, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.AddDays(365).ToString("yyyy-MM-dd HH:mm:ss"));


                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Registered Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Insert_Registered(RegisterModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(
                                    @"INSERT INTO SYS_REGISTER 
                                    ([system_id],[name_eng],[name_thai],[address],[email],[website],[tel],[fax],[contact],[country],[register_date]) 
                                    VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                    data.system_id, data.name_eng, data.name_thai, data.address,
                                    data.email, data.website, data.tel, data.fax, data.contact,
                                    data.country, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Insert Registered Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Update_Registered(RegisterModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(
                                    @"UPDATE SYS_REGISTER SET
                                    [name_eng]='{1}',[name_thai]='{2}',[address]='{3}',[email]='{4}',
                                    [website]='{5}',[tel]='{6}',[fax]='{7}',[contact]='{8}',[country]='{9}'
                                    WHERE system_id='{0}'",
                                    data.system_id, data.name_eng, data.name_thai, data.address, data.email,
                                    data.website, data.tel, data.fax, data.contact, data.country);


                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Update Registered Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

        public int Delete_Registered(RegisterModel data)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"DELETE FROM SYS_REGISTER WHERE system_id='{0}'", data.system_id);

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Delete Registered Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }

    }
}