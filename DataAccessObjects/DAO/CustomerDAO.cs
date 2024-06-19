using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects.DAO
{
    public class CustomerDAO
    {
        private static CustomerDAO instance = null;
        public static readonly object Lock = new object();
        private CustomerDAO() { }
        public static CustomerDAO Instance
        {
            get
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new CustomerDAO();
                    }
                    return instance;
                }
            }
        }
        public async Task<List<Customer>> GetCustomers()
        {
            using var db = new FuminiHotelManagementContext();
            return await db.Customers.ToListAsync();
        }

        public async Task<Customer> CheckLogin(string email, string password)
        {
            using var db = new FuminiHotelManagementContext();
            return await db.Customers.SingleOrDefaultAsync(m => m.EmailAddress.Equals(email) && m.Password.Equals(password));
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customers = await GetCustomers();
            return customers.SingleOrDefault(m => m.CustomerId == id);
        }

        public async Task Register(Customer cus)
        {
            using var db = new FuminiHotelManagementContext();
            await db.Customers.AddAsync(cus);
            await db.SaveChangesAsync();
        }

        public async Task RemoveCustomer(Customer cus)
        {
            using var db = new FuminiHotelManagementContext();
            var c1 = await db.Customers.SingleOrDefaultAsync(m => m.CustomerId == cus.CustomerId);
            if (c1 != null)
            {
                db.Customers.Remove(c1);
                await db.SaveChangesAsync();
            }
        }
    }
}
