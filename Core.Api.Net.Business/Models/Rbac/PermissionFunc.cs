using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class PermissionFuncModel
    {
        public int seq { get; set; }
        public string id { get; set; }
        public string group_code { get; set; }
        public string group_name { get; set; }
        public string func_code { get; set; }
        public string func_name { get; set; }
        public string func_type { get; set; }
        public string func_ref_sub { get; set; }
        public bool used { get; set; }
        public DateTime? create_date { get; set; }
    }

    public class ResponsePermissionFunc
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<PermissionFuncModel> data { get; set; }
    }

}