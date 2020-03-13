using bFit.Web.Data.Entities.Workouts;
using System.Collections.Generic;

namespace bFit.Web.Models
{
    public class SetViewModel : SubSetViewModel
    {
        public ICollection<SubSet> Subsets { get; set; }
    }
}
