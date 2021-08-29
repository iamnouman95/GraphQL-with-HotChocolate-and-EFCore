using GraphQL_HotChoclate_EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL_HotChoclate_EFCore.Services
{
    public interface ICustomerService
    {
        CustomerViewModel Create(CustomerViewModel customer);
        CustomerViewModel Update(CustomerViewModel customer);
        bool Delete(CustomerViewModel deleteVM);
        IQueryable<Customer> GetAll();
    }
}
