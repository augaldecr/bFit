using System.Collections.Generic;

namespace bFit.Web.Data.Entities.Workouts
{
    public class SetTemplate : IEntity
    {
        public int Id { get; set; }

        public Template Template { get; set; }

        public virtual ICollection<SubSetTemplate> SubSetTemplates { get; set; }
    }
}
