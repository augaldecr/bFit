namespace bFit.Web.Data.Entities.Profiles
{
    public class Membership : IEntity
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public LocalGym LocalGym  { get; set; }
        public bool Active { get; set; }

        public override string ToString()
        {
            return $"{Customer.User.FullName}";
        }
    }
}