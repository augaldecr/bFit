using bFit.WEB.Data.Entities;
using bFit.WEB.Data.Entities.Profiles;

namespace bFit.Web.Data.Entities.Profiles
{
    interface IFranchiseEmployee
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Franchise Franchise { get; set; }
    }
}
