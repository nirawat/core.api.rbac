using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class OwnerModel
    {
        public string code { get; set; }
        public string name_thai { get; set; }
        public string name_eng { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string website { get; set; }
        public string tel { get; set; }
        public string mobile_phone { get; set; }
        public string contact { get; set; }
        public string country { get; set; }
    }

}