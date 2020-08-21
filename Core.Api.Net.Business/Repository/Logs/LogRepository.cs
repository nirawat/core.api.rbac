using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Logs
{
    public class LogRepository : ILogRepository
    {
        private readonly ILogs _ILogs;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public LogRepository(ILogs ILogs, IEnvironmentConfigs IEnvironmentConfigs)
        {
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public int WriteLogs(string log_event, string account, string token, string message)
        {
            int resp = -1;
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"INSERT INTO SYS_EVENT_LOG 
                                                (log_account,log_token,log_message,log_event,log_system_id,log_event_datetime) 
                                                VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')",
                                                account, token, message, log_event, "RBAC", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    int rest = mssql.ExcuteNonQueryStr(query);

                    if (rest >= 0) { resp = 0; }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("WriteLogs: ", ex.Message.ToString(), ex.StackTrace);
            }
            return resp;
        }


    }
}