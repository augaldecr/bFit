using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Models;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Customer ToCustomer(CustomerViewModel model);
        CustomerViewModel ToCustomerViewModel(Customer customer);
    }
}