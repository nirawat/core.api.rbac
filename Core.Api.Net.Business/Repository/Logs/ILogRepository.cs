using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Logs
{
    public interface ILogRepository
    {
        int WriteLogs(string log_event, string account_id, string token, string message);
    }
}