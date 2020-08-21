using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

public class UsageStatusModel
{
    public int online_count { get; set; }
    public string online_percent { get; set; }
    public int offline_count { get; set; }
    public string offline_percent { get; set; }
}

public class ResponseUsageStatus
{
    public StatusResponse status { get; set; } = StatusResponse.Error;
    public string message { get; set; }
    public IList<UsageStatusModel> data { get; set; }
}

public class StaticChartModel
{
    public string event_time { get; set; }
    public int event_time_count { get; set; }
}

public class ResponseRequestStatisticLog
{
    public StatusResponse status { get; set; } = StatusResponse.Error;
    public string message { get; set; }
    public IList<StaticChartModel> data { get; set; }
}

public class StatisticSignInLogModel
{
    public int event_day { get; set; }
    public int event_day_count { get; set; }
}

public class ResponseStatisticSignInLog
{
    public StatusResponse status { get; set; } = StatusResponse.Error;
    public string message { get; set; }
    public IList<StatisticSignInLogModel> data { get; set; }
}

public class UsageOfCountryModel
{
    public string country { get; set; }
    public string usage_count { get; set; }
}

public class ResponseUsageOfCountry
{
    public StatusResponse status { get; set; } = StatusResponse.Error;
    public string message { get; set; }
    public IList<UsageOfCountryModel> data { get; set; }
}

