using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Section
{
    public interface ISectionRepository
    {
        IList<SectionModel> GetListSection();
        SectionModel GetSection(string code);
        int Insert_Section(SectionModel data);
        int Update_Section(SectionModel data);
        int Delete_Section(SectionModel data);

    }
}