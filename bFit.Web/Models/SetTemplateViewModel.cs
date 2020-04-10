using bFit.Web.Data.Entities.Workouts;
using System.Collections.Generic;

namespace bFit.Web.Models
{
    public class SetTemplateViewModel : SubSetTemplateViewModel
    {
        public ICollection<SubSetTemplate> SubSetTemplates { get; set; }
    }
}
