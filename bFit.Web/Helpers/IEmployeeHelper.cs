using bFit.WEB.Data.Entities;

namespace bFit.Web.Helpers
{
    public interface IEmployeeHelper
    {
        IEntity EmployeeAsync(string email);
    }
}