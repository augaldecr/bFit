using bFit.Web.Data.Entities.Profiles;

namespace bFit.Web.Helpers
{
    public interface IEmployeeHelper
    {
        IFranchiseEmployee EmployeeAsync(string email);
    }
}