
namespace Core.Api.Net.Business.Models.Response
{
    public class RefreshToken
    {
        public StatusResponse status { get; set; } = StatusResponse.Error;
        public string message { get; set; }
        public string token { get; set; }
    }
}