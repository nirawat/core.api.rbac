using System;
using System.Collections.Generic;
using System.Data;
using Core.Api.Net.Business.Helpers.DBAccess;
using Core.Api.Net.Business.Helpers.Environment;
using Core.Api.Net.Business.Helpers.Logs;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Services.Section
{
    public interface ISectionService
    {
        IList<SectionModel> GetListSection();
        SectionModel GetSection(string code);
        ResponseSection Insert_Section(SectionModel data);
        ResponseSection Update_Section(SectionModel data);
        ResponseSection Delete_Section(SectionModel data);
    }
}