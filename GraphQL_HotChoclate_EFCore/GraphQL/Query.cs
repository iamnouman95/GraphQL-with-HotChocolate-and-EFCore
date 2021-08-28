using GraphQL_HotChoclate_EFCore.Models;
using GraphQL_HotChoclate_EFCore.Services;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GraphQL_HotChoclate_EFCore.Enums.StatusEnum;

namespace GraphQL_HotChoclate_EFCore.GraphQL
{
    public class Query
    {
        #region Property  
        private readonly ICustomerService _customerService;
        #endregion

        #region Constructor  
        public Query(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        #endregion

        /// <summary>
        /// Using filter and sorting options of HotChocolate.Data
        /// </summary>
        /// 
        [UseProjection]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> Customers => _customerService.GetAll();


        /// <summary>
        /// Filter by name, email and status
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <returns>json string of customers matched with the parameters</returns>
        public IQueryable<Customer> CustomersBy(String name, String email, Status status) 
            => _customerService.GetAll().Where(x => x.Name == name 
                                                        && x.Email == email
                                                        && x.Status == status);
        
    }
}
