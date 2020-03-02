using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using System.Linq;

namespace bFit.Web.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private readonly ApplicationDbContext _context;

        public EmployeeHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IFranchiseEmployee EmployeeAsync(string email)
        {
            var franAdm = _context.FranchiseAdmins.FirstOrDefault(e => e.User.Email == email);
            if (franAdm != null)
            {
                return (IFranchiseEmployee)franAdm;
            }

            var gymAdm = _context.GymAdmins.FirstOrDefault(e => e.User.Email == email);
            if (gymAdm != null)
            {
                return (IFranchiseEmployee)gymAdm;
            }

            var trainer = _context.Trainers.FirstOrDefault(e => e.User.Email == email);
            if (trainer != null)
            {
                return (IFranchiseEmployee)trainer;
            }

            return null;
        }
    }
}
