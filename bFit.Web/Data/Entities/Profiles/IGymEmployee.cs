using bFit.WEB.Data.Entities.Workouts;

namespace bFit.WEB.Data.Entities.Profiles
{
    public interface IGymEmployee
    {
        public int Id { get; set; }
        public User User { get; set; }
        public LocalGym Gym { get; set; }
    }
}