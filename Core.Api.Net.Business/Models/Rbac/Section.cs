using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class SectionModel
    {
        public string code { get; set; }
        public string name_thai { get; set; }
        public string name_eng { get; set; }
    }

    public class ResponseSection
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<SectionModel> data { get; set; }
    }

}