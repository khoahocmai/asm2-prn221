using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessObjects.DAO;

namespace Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<List<Customer>> GetCustomers()
           => CustomerDAO.Instance.GetCustomers();

        public Task<Customer> CheckLogin(string email, string password)
            => CustomerDAO.Instance.CheckLogin(email, password);

        public Task<Customer> GetCustomerById(int id)
            => CustomerDAO.Instance.GetCustomerById(id);

        public Task Register(Customer cus)
            => CustomerDAO.Instance.Register(cus);

        public Task RemoveCustomer(Customer cus)
            => CustomerDAO.Instance.RemoveCustomer(cus);
    }
}
