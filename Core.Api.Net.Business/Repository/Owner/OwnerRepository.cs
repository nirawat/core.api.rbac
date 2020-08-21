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

namespace Core.Api.Net.Business.Repository.Owner
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ILogs _ILogs;
        private readonly IHttpContextAccessor _IHttpContextAccessor;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public OwnerRepository(ILogs ILogs, IHttpContextAccessor IHttpContextAccessor, IEnvironmentConfigs IEnvironmentConfigs)
        {
            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public OwnerModel GetOwner()
        {
            OwnerModel resp = new OwnerModel();
            try
            {
                string query = string.Format(@"SELECT * FROM SYS_OWNER");

                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            resp.code = dr["code"].ToString();
                            resp.name_eng = dr["name_eng"].ToString();
                            resp.name_thai = dr["name_thai"].ToString();
                            resp.address = dr["address"].ToString();
                            resp.email = dr["email"].ToString();
                            resp.website = dr["website"].ToString();
                            resp.tel = dr["tel"].ToString();
                            resp.mobile_phone = dr["mobile_phone"].ToString();
                            resp.contact = dr["contact"].ToString();
                            resp.country = dr["country"].ToString();
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Get Owner Repository: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}