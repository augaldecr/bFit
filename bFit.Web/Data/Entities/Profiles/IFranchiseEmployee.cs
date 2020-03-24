namespace bFit.Web.Data.Entities.Profiles
{
    public interface IFranchiseEmployee : IEntity
    {
        public User User { get; set; }
        public Franchise Franchise { get; }
    }
}
