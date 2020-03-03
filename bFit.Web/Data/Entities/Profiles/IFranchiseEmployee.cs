using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Profiles;

namespace bFit.Web.Data.Entities.Profiles
{
    public interface IFranchiseEmployee : IEntity
    {
        public User User { get; set; }
        public Franchise Franchise { get; set; }
    }
}
