using System.Collections.Generic;

namespace Core.Api.Net.Business.Services.Dashboard
{
    public interface IDashboardService
    {
        IList<UsageStatusModel> GetUsageStatusLogs(string _date);
        IList<StaticChartModel> GetStatisticRequestTokenLogs(string _date);
        IList<StatisticSignInLogModel> GetStatisticUsageLogs(int _month, int _year);
        IList<UsageOfCountryModel> GetUsageOfCountry();
    }
}