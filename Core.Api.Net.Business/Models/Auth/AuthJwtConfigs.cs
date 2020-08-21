using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Api.Net.Business.Models.Auth
{
    public interface IJwtConfigs
    {
        string Secret { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int Expires { get; set; }
        int ExpiresRegister { get; set; }
    }
    public class JwtConfigs : IJwtConfigs
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expires { get; set; }
        public int ExpiresRegister { get; set; }
    }
}
