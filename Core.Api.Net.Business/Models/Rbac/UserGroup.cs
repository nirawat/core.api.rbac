using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class UserGroupModel
    {
        public string code { get; set; }
        public string name_thai { get; set; }
        public string name_eng { get; set; }
        public int group_level { get; set; }
        public int count_member { get; set; }
        public int count_role { get; set; }
        public int count_func { get; set; }
        public string theme_code { get; set; }
        public DateTime create_date { get; set; }
    }

    public class ResponseUserGroup
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<UserGroupModel> data { get; set; }
    }

}