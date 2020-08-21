using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class PermissionGroupModel
    {
        public int seq { get; set; }
        public string id { get; set; }
        public string group_code { get; set; }
        public string group_name { get; set; }
        public string account_id { get; set; }
        public string account_full_name { get; set; }
        public string account_department { get; set; }
        public bool used { get; set; }
        public DateTime? create_date { get; set; }
    }

    public class ResponsePermissionGroup
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<PermissionGroupModel> data { get; set; }
    }

}