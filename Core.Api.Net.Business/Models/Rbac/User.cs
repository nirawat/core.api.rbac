using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class UserModel
    {
        public string account_id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string prefixes { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string language { get; set; }
        public string section_code { get; set; }
        public string section_name { get; set; }
        public DateTime? activate_date { get; set; }
    }

    public class ResponseUser
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public string account_id { get; set; }
        public IList<UserModel> data { get; set; }
    }

}