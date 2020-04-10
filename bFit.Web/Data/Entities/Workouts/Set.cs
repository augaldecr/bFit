using System.Collections.Generic;

namespace bFit.Web.Data.Entities.Workouts
{
    public class Set : IEntity
    {
        public int Id { get; set; }

        public WorkoutRoutine WorkoutRoutine { get; set; }

        public virtual ICollection<SubSet> SubSets { get; set; }
    }
}
