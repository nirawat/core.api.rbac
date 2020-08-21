using System.Collections.Generic;

namespace Core.Api.Net.Business.Repository.Dashboard
{
    public interface IDashboardRepository
    {
        IList<UsageStatusModel> GetUsageStatusLogs(string _date);
        IList<StaticChartModel> GetStatisticRequestTokenLogs(string _date);
        IList<StatisticSignInLogModel> GetStatisticUsageLogs(int _month, int _year);
        IList<UsageOfCountryModel> GetUsageOfCountry();
    }
}