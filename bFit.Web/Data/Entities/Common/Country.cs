using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bFit.WEB.Data.Entities.Common
{
    //País
    public class Country : BasicEntity, IEntity
    {
        [Display(Name = "Provincias")]
        public virtual ICollection<State> States { get; set; }
    }
}