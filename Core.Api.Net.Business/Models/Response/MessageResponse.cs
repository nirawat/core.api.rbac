using System;
using System.Collections.Generic;

namespace Core.Api.Net.Business.Models.Response
{

    public class MessageResponse
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
    }

    public enum StatusResponse
    {
        Error = 0,
        Success = 200,
    }

}