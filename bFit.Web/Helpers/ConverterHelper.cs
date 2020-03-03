using bFit.Web.Data.Entities.Profiles;
using bFit.Web.Models;

namespace bFit.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public CustomerViewModel ToCustomerViewModel(Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                SocialSecurity = customer.User.SocialSecurity,
                FirstName = customer.User.FirstName,
                LastName1 = customer.User.LastName1,
                LastName2 = customer.User.LastName2,
                CellPhone = customer.User.PhoneNumber,
                Email = customer.User.Email,
                Birthday = customer.Birthday,
                Gender = customer.Gender,
                Town = customer.User.Town,
                Address = customer.User.Address,
            };
        }

        public Customer ToCustomer(CustomerViewModel model)
        {
            return new Customer
            {
                Id = model.Id,
                Birthday = model.Birthday,
                Gender = model.Gender,
            };
        }
    }
}
