using System.Collections.Generic;
using Core.Api.Net.Business.Models.Rbac;

namespace Core.Api.Net.Business.Repository.Owner
{
    public interface IOwnerRepository
    {
        OwnerModel GetOwner();
    }
}