using GraphQL_HotChoclate_EFCore.GraphQL;
using GraphQL_HotChoclate_EFCore.Models;
using GraphQL_HotChoclate_EFCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static GraphQL_HotChoclate_EFCore.Enums.StatusEnum;

namespace WebApi_Tests
{
    public class GraphQL_WebAPI_Tests
    {
        Query _queryController;
        Mutation _mutController;
        ICustomerService _service;
        DatabaseContext databaseContext;

        public DbContextOptions<DatabaseContext> dbContextOptions { get; }
        public IConfigurationRoot Configuration { get; set; }

        public GraphQL_WebAPI_Tests()
        {
            //Read appsettings.json
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            //Create dbContext with connection string
            dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(Configuration.GetConnectionString("myconn"))
                .Options;

            //Inject services
            databaseContext = new DatabaseContext(dbContextOptions);
            _service = new CustomerService(databaseContext);
            _queryController = new Query(_service);
            _mutController = new Mutation(_service);
        }

        /// <summary>
        /// This test will check if the data retrieved from database
        /// is null or not
        /// </summary>
        [Fact]
        public void Get_WhenCalled_ReturnNotNull()
        {
            //Act
            var okResult = _queryController.Customers;

            //Assert
            Assert.NotNull(okResult);
        }

        /// <summary>
        /// Thus test will check if the Name column
        /// exceeds maximum input length of 128 char
        /// </summary>
        [Fact]
        public void Post_WhenCalled_ReturnNameStringLengthInvalid()
        {
            //Arrange
            var testItem = new CustomerViewModel()
            {
                Email = "dummy@gmail.com",
                Name = "This is a string with more than max length 128 " +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz",
                Code = null,
                Status = Status.Active,
                CreatedAt = DateTime.Now,
                IsBlocked = false
            };

            try
            {
                var okResult = _mutController.Create(testItem);
            }
            catch(Exception e)
            {
                //Assert
                Assert.IsType<DbUpdateException>(e);
            }

        }

        /// <summary>
        /// This test will check if the Email column
        /// exceeds maximum input length of 128 char
        /// </summary>
        [Fact]
        public void Post_WhenCalled_ReturnEmailStringLengthInvalid()
        {
            //Arrange
            var testItem = new CustomerViewModel()
            {
                Email = "This is a string with more than max length 128 " +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz" +
                        "abcdefghijklmnopqrstuvwxyz@gmail.com",
                Name = "Dummy",
                Code = null,
                Status = Status.Active,
                CreatedAt = DateTime.Now,
                IsBlocked = false
            };

            try
            {
                var okResult = _mutController.Create(testItem);
            }
            catch (Exception e)
            {
                //Assert
                Assert.IsType<DbUpdateException>(e);
            }

        }

        /// <summary>
        /// This test will check if Create operation 
        /// of WebApi is working fine
        /// </summary>
        [Fact]
        public void Post_WhenCalled_ReturnNewCustomer()
        {
            //Arrange
            var testItem = new CustomerViewModel()
            {
                Email = "dummy@gmail.com",
                Name = "Dummy",
                Code = null,
                Status = Status.Active,
                CreatedAt = DateTime.Now,
                IsBlocked = false
            };

            //Act
            var cust = databaseContext.Customer.Where(x => x.Name.Contains("Dummy"));
            if(cust.Count() != 0)
            {
                databaseContext.RemoveRange(cust);
                databaseContext.SaveChanges();
            }

            var okResult = _mutController.Create(testItem);

            //Assert
            Assert.IsType<CustomerViewModel>(okResult);
            Assert.Equal("Dummy", okResult.Name);
        }

        /// <summary>
        /// This test will check if Update operation
        /// of WebApi is working fine
        /// </summary>
        [Fact]
        public void Post_WhenCalled_ReturnUpdatedCustomer()
        {
            //Arrange
            var cust = _queryController.Customers.FirstOrDefault(x=>x.Name == "Dummy");
            if(cust is not null)
            {
                var testItem = new CustomerViewModel()
                {
                    Id = cust.Id,
                    Name = "Dummy" + (cust.Id + 1),
                };

                //Act
                var okResult = _mutController.Update(testItem);

                //Assert
                Assert.IsType<CustomerViewModel>(okResult);
                Assert.Equal("Dummy" + (cust.Id + 1), okResult.Name);
            }
        }

        /// <summary>
        /// This test will check if Delete operation
        /// of WebApi is working fine
        /// </summary>
        [Fact]
        public void Post_WhenCalled_ReturnDeletedCustomerStatus()
        {
            //Arrange
            var cust = _queryController.Customers.FirstOrDefault(x => x.Name.Contains("Dummy"));
            if(cust is not null)
            {
                var testItem = new CustomerViewModel()
                {
                    Id = cust.Id
                };

                //Act
                var okResult = _mutController.Delete(testItem);

                //Assert
                Assert.True(okResult);
            }
        }
    }
}
