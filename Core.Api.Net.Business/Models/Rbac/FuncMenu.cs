using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class FuncListModel
    {
        public List<FuncModel> func { get; set; }
    }

    public class FuncModel
    {
        public int seq { get; set; }
        public string key { get; set; }
        public string icon { get; set; }
        public string path { get; set; }
        public bool divider { get; set; }
        public IList<SubFuncModel> sub_item { get; set; }
    }

    public class SubFuncModel
    {
        public int seq { get; set; }
        public string key { get; set; }
        public string func_name { get; set; }
        public string icon { get; set; }
        public string path { get; set; }
    }

    public class FunctionModel
    {
        public string code { get; set; }
        public string name_thai { get; set; }
        public string name_eng { get; set; }
        public string route_path { get; set; }
        public string icon_name { get; set; }
        public string func_type { get; set; }
        public string func_ref_sub { get; set; }
    }

    public class ResponseFunction
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<FunctionModel> data { get; set; }
    }
}