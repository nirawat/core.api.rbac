using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Configs;

namespace Core.Api.Net.Business.Repository.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ILogs _ILogs;
        private readonly IEnvironmentConfigs _IEnvironmentConfigs;
        private EnvironmentModel _EnvironmentModel;
        public DashboardRepository(ILogs ILogs, IEnvironmentConfigs IEnvironmentConfigs)
        {
            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;


            _ILogs = ILogs;
            _IEnvironmentConfigs = IEnvironmentConfigs;
            _EnvironmentModel = _IEnvironmentConfigs.GetEnvironmentSetting();
        }

        public IList<UsageStatusModel> GetUsageStatusLogs(string _date)
        {
            IList<UsageStatusModel> resp = new List<UsageStatusModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"SELECT 0 as account_status, 
                                                ISNULL(COUNT(account_id),0) as account_count
                                                FROM [dbo].[SYS_ACCOUNT]
                                                UNION
                                                SELECT 1 as account_status, 
                                                ISNULL(COUNT(account_id),0) as account_count
                                                FROM [dbo].[SYS_ACCOUNT]
                                                WHERE online_expire IS NOT NULL
                                                UNION
                                                SELECT 2 as account_status, 
                                                ISNULL(COUNT(account_id),0) as account_count 
                                                FROM [dbo].[SYS_ACCOUNT]
                                                WHERE online_expire IS NULL
                                                ORDER BY account_status ASC");

                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        UsageStatusModel usage_status = new UsageStatusModel();
                        int _all = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToInt32(dr["account_status"]) == 0)
                            {
                                _all = Convert.ToInt32(dr["account_count"]);
                            }
                            if (Convert.ToInt32(dr["account_status"]) == 1)
                            {
                                usage_status.online_count = Convert.ToInt32(dr["account_count"]);
                                usage_status.online_percent = string.Format(@"{0}%", (100 * usage_status.online_count / _all));
                            }
                            if (Convert.ToInt32(dr["account_status"]) == 2)
                            {
                                usage_status.offline_count = Convert.ToInt32(dr["account_count"]);
                                usage_status.offline_percent = string.Format(@"{0}%", (100 * usage_status.offline_count / _all));
                            }
                        }
                        resp.Add(usage_status);
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("WriteLogs: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }
        public IList<StaticChartModel> GetStatisticRequestTokenLogs(string _date)
        {
            IList<StaticChartModel> resp = new List<StaticChartModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"SELECT 
                                                REPLACE(CONVERT(VARCHAR(5),log_event_datetime,108),':','.') AS event_time,
                                                COUNT(CONVERT(VARCHAR(5),log_event_datetime,108)) AS event_time_count
                                                FROM SYS_EVENT_LOG 
                                                WHERE (log_token IS NOT NULL OR log_token <>'') AND CONVERT(VARCHAR,log_event_datetime,103) = '{0}'
                                                GROUP BY CONVERT(VARCHAR(5),log_event_datetime,108)
                                                ORDER BY CONVERT(VARCHAR(5),log_event_datetime,108) ASC",
                                                _date);

                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            StaticChartModel token_model = new StaticChartModel()
                            {
                                event_time = dr["event_time"].ToString(),
                                event_time_count = Convert.ToInt32(dr["event_time_count"]),
                            };
                            resp.Add(token_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("WriteLogs: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<StatisticSignInLogModel> GetStatisticUsageLogs(int _month, int _year)
        {
            IList<StatisticSignInLogModel> resp = new List<StatisticSignInLogModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    string query = string.Format(@"SELECT 
                                                DAY(log_event_datetime) AS event_day,
                                                COUNT(CONVERT(VARCHAR(5),log_event_datetime,103)) AS event_day_count
                                                FROM SYS_EVENT_LOG 
                                                WHERE (log_token IS NOT NULL OR log_token <>'') 
                                                AND MONTH(log_event_datetime)={0} AND YEAR(log_event_datetime)={1}
                                                GROUP BY DAY(log_event_datetime)
                                                ORDER BY DAY(log_event_datetime) ASC",
                                                _month, _year);

                    var dt = mssql.GetDataTableFromQueryStr(query);

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 1; i < 31; i++)
                        {
                            StatisticSignInLogModel sign_in_model = new StatisticSignInLogModel();
                            sign_in_model.event_day = i;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToInt32(dr["event_day"]) == i)
                                {
                                    sign_in_model.event_day_count = Convert.ToInt32(dr["event_day_count"]);
                                }
                            }
                            resp.Add(sign_in_model);
                        }
                        return resp;
                    }
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("WriteLogs: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<UsageOfCountryModel> GetUsageOfCountry()
        {
            IList<UsageOfCountryModel> resp = new List<UsageOfCountryModel>();
            try
            {
                using (MSSql mssql = new MSSql(DBConnectionType.RBAC, _EnvironmentModel))
                {
                    UsageOfCountryModel country_Germany = new UsageOfCountryModel() { country = "Germany", usage_count = 200.ToString() };
                    UsageOfCountryModel country_USA = new UsageOfCountryModel() { country = "United States", usage_count = 300.ToString() };
                    UsageOfCountryModel country_Brazil = new UsageOfCountryModel() { country = "Brazil", usage_count = 400.ToString() };
                    UsageOfCountryModel country_Canada = new UsageOfCountryModel() { country = "Canada", usage_count = 500.ToString() };
                    UsageOfCountryModel country_France = new UsageOfCountryModel() { country = "France", usage_count = 600.ToString() };
                    UsageOfCountryModel country_Rusia = new UsageOfCountryModel() { country = "RU", usage_count = 700.ToString() };
                    UsageOfCountryModel country_Thailand = new UsageOfCountryModel() { country = "Thailand", usage_count = 800.ToString() };

                    resp.Add(country_Germany);
                    resp.Add(country_USA);
                    resp.Add(country_Brazil);
                    resp.Add(country_Canada);
                    resp.Add(country_France);
                    resp.Add(country_Rusia);
                    resp.Add(country_Thailand);

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _ILogs.LogError("WriteLogs: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

    }
}