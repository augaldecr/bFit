using bFit.Web.Data;
using bFit.Web.Data.Entities.Profiles;
using bFit.WEB.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bFit.Web.Helpers
{
    public class EmployeeHelper : IEmployeeHelper
    {
        private readonly ApplicationDbContext _context;

        public EmployeeHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEntity EmployeeAsync(string email)
        {
            var franAdm = _context.FranchiseAdmins.FirstOrDefault(e => e.User.Email == email);
            if (franAdm != null)
            {
                return (IEntity)franAdm;
            }

            var gymAdm = _context.GymAdmins.FirstOrDefault(e => e.User.Email == email);
            if (gymAdm != null)
            {
                return (IEntity)gymAdm;
            }

            var trainer = _context.Trainers.FirstOrDefault(e => e.User.Email == email);
            if (trainer != null)
            {
                return (IEntity)trainer;
            }

            return null;
        }
    }
}
