using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Repository.Dashboard;

namespace Core.Api.Net.Business.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ILogs _ILogs;
        private readonly IDashboardRepository _IDashboardRepository;
        public DashboardService(ILogs ILogs, IDashboardRepository IDashboardRepository)
        {
            _ILogs = ILogs;
            _IDashboardRepository = IDashboardRepository;
        }

        public IList<UsageStatusModel> GetUsageStatusLogs(string _date)
        {
            IList<UsageStatusModel> resp = new List<UsageStatusModel>();
            try
            {
                resp = _IDashboardRepository.GetUsageStatusLogs(_date);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Dashboard Usage Status Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }
        public IList<StaticChartModel> GetStatisticRequestTokenLogs(string _date)
        {
            IList<StaticChartModel> resp = new List<StaticChartModel>();
            try
            {
                resp = _IDashboardRepository.GetStatisticRequestTokenLogs(_date);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Dashboard Statistic Request Token Log Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<StatisticSignInLogModel> GetStatisticUsageLogs(int _month, int _year)
        {
            IList<StatisticSignInLogModel> resp = new List<StatisticSignInLogModel>();
            try
            {
                resp = _IDashboardRepository.GetStatisticUsageLogs(_month, _year);

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Dashboard Statistic Usage Log Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }

        public IList<UsageOfCountryModel> GetUsageOfCountry()
        {
            IList<UsageOfCountryModel> resp = new List<UsageOfCountryModel>();
            try
            {
                resp = _IDashboardRepository.GetUsageOfCountry();

                return resp;
            }
            catch (Exception ex)
            {
                _ILogs.LogError("Dashboard Usage of country Service: ", ex.Message.ToString(), ex.StackTrace);
            }
            return null;
        }
    }
}