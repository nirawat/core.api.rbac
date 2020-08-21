using System;
using Core.Api.Net.Business.Models.Configs;

namespace Core.Api.Net.Business.Helpers.Environment
{
    public interface IEnvironmentConfigs
    {
        EnvironmentModel GetEnvironmentSetting();
    }
    public class EnvironmentConfigs : IEnvironmentConfigs
    {
        private readonly EnvironmentModel _EnvironmentModel;
        public EnvironmentConfigs(EnvironmentModel EnvironmentModel)
        {
            _EnvironmentModel = EnvironmentModel;
        }
        public EnvironmentModel GetEnvironmentSetting()
        {
            return _EnvironmentModel;
        }
    }
}