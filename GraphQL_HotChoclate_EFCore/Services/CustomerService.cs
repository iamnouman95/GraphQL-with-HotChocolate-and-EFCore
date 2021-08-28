using GraphQL_HotChoclate_EFCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL_HotChoclate_EFCore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DatabaseContext _dbContext;

        public CustomerService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public CustomerViewModel Create(CustomerViewModel customer)
        {
            var data = new Customer();
            data.Email = customer.Email;
            data.Name = customer.Name;
            data.Code = customer.Code;
            if (customer.Status is not null)
            {
                data.Status = (Enums.StatusEnum.Status)customer.Status;
            }

            data.CreatedAt = customer.CreatedAt is not null ? (DateTime)customer.CreatedAt : DateTime.Now;

            if (customer.IsBlocked is not null)
            {
                data.IsBlocked = (bool)customer.IsBlocked;
            }

            _dbContext.Customer.Add(data);
            _dbContext.SaveChanges();

            var newRec = _dbContext.Customer.OrderByDescending(x => x.Id).FirstOrDefault();
            customer.Id = newRec.Id;
            customer.CreatedAt = Convert.ToDateTime(newRec.CreatedAt.ToString().Substring(0, 10));

            return customer;
        }

        public CustomerViewModel Update(CustomerViewModel customer)
        {
            var cust = _dbContext.Customer.FirstOrDefault(c => c.Id == customer.Id);
            if (cust is not null)
            {
                if (!String.IsNullOrEmpty(customer.Email))
                {
                    cust.Email = customer.Email;
                }
                if (!String.IsNullOrEmpty(customer.Name))
                {
                    cust.Name = customer.Name;
                }
                if (customer.Code is not null && customer.Code != 0)
                {
                    cust.Code = customer.Code;
                }
                if (customer.Status is not null)
                {
                    cust.Status = (Enums.StatusEnum.Status)customer.Status;
                }
                if (customer.CreatedAt is not null)
                {
                    cust.CreatedAt = (DateTime)customer.CreatedAt;
                }
                if (customer.IsBlocked is not null)
                {
                    cust.IsBlocked = (bool)customer.IsBlocked;
                }

                _dbContext.SaveChanges();
            }

            var newRec = _dbContext.Customer.FirstOrDefault(x => x.Id == customer.Id);
            customer.Id = newRec.Id;
            customer.Email = newRec.Email;
            customer.Name = newRec.Name;
            customer.Code = newRec.Code;
            customer.Status = newRec.Status;
            customer.IsBlocked = newRec.IsBlocked;
            customer.CreatedAt = Convert.ToDateTime(newRec.CreatedAt.ToString().Substring(0, 10));

            return customer;
        }

        public bool Delete(DeleteVM deleteVM)
        {
            var customer = _dbContext.Customer.FirstOrDefault(c => c.Id == deleteVM.Id);
            if (customer is not null)
            {
                _dbContext.Customer.Remove(customer);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteByName(DeleteVM deleteVM)
        {

            var customer = _dbContext.Customer.FirstOrDefault(c => c.Name == deleteVM.Name);
            if (customer is not null)
            {
                _dbContext.Customer.Remove(customer);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public IQueryable<Customer> GetAll()
        {
            return _dbContext.Customer.AsQueryable();
        }
    }

    public class DeleteVM
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
