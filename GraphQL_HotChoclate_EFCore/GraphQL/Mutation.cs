using GraphQL_HotChoclate_EFCore.Models;
using GraphQL_HotChoclate_EFCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL_HotChoclate_EFCore.GraphQL
{
    public class Mutation
    {
        #region Property  
        private readonly ICustomerService _customerService;
        #endregion

        #region Constructor  
        public Mutation(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion
        public CustomerViewModel Create(CustomerViewModel customer) => _customerService.Create(customer);
        public CustomerViewModel Update(CustomerViewModel customer) => _customerService.Update(customer);
        public bool Delete(DeleteVM deleteVM) => _customerService.Delete(deleteVM);
        public bool DeleteByName(DeleteVM deleteVM) =>_customerService.DeleteByName(deleteVM);
    }
}
