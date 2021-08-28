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
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(Configuration.GetConnectionString("myconn"))
                .Options;

            databaseContext = new DatabaseContext(dbContextOptions);
            _service = new CustomerService(databaseContext);
            _queryController = new Query(_service);
            _mutController = new Mutation(_service);
        }

        [Fact]
        public void Get_WhenCalled_ReturnNotNull()
        {
            //Act
            var okResult = _queryController.Customers;

            //Assert
            Assert.NotNull(okResult);
        }

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

            var delete = new DeleteVM()
            {
                Name = "Dummy"
            };

            //Act
            _mutController.DeleteByName(delete);
            var okResult = _mutController.Create(testItem);

            //Assert
            Assert.IsType<CustomerViewModel>(okResult);
            Assert.Equal("Dummy", okResult.Name);
        }


    }
}
