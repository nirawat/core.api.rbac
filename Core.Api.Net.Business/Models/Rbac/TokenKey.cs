using System;
using System.Collections.Generic;
using Core.Api.Net.Business.Models.Response;

namespace Core.Api.Net.Business.Models.Rbac
{
    public class TokenKeyModel
    {
        public string code { get; set; }
        public string secret { get; set; }
        public string issuer { get; set; }
        public string audience { get; set; }
        public int expire { get; set; }
        public DateTime create_date { get; set; }
    }

    public class ResponseTokenKey
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public IList<TokenKeyModel> data { get; set; }
    }

}