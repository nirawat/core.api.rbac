using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class RegisterModel
    {
        public string register_id { get; set; }
        public string system_id { get; set; }
        public string token { get; set; }
        public string name_thai { get; set; }
        public string name_eng { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string contact { get; set; }
        public string country { get; set; }
        public DateTime register_date { get; set; }
        public DateTime activate_date { get; set; }
        public DateTime expire_date { get; set; }
        public bool activate { get; set; }
    }

    public class ResponseRegister
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public string register_id { get; set; }
        public string token { get; set; }
        public IList<RegisterModel> data { get; set; }
    }

}