using System;
using NLog;

namespace Core.Api.Net.Business.Helpers.Logs
{
    public interface ILogs
    {
        void LogInfor(string title, string message);

        void LogError(string title, string message, string stacktrace);
    }
    
    public class Logs : ILogs
    {
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public Logs()
        {
        }
        public void LogInfor(string title, string message)
        {
            string dt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string log = $"Info : \r\n{title} {dt}\r\n{message}";
            _logger.Info(log);
        }
        public void LogError(string title, string message, string stacktrace)
        {
            string dt = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            string log = $"Error : \r\n{title} {dt}\r\n{message}\r\n{stacktrace}";
            _logger.Info(log);
        }
    }
}