using bFit.Web.Models;
using bFit.WEB.Data.Entities.Profiles;

namespace bFit.Web.Helpers
{
    public interface IConverterHelper
    {
        Customer ToCustomer(CustomerViewModel model);
        CustomerViewModel ToCustomerViewModel(Customer customer);
    }
}