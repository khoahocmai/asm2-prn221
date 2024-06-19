using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetCustomers();
        Task<Customer> CheckLogin(string email, string password);
        Task<Customer> GetCustomerById(int id);
        Task Register(Customer cus);
        Task RemoveCustomer(Customer cus);
    }
}
