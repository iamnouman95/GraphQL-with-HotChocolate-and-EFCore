using GraphQL_HotChoclate_EFCore.GraphQL;
using GraphQL_HotChoclate_EFCore.Models;
using GraphQL_HotChoclate_EFCore.Services;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace GraphQL_HotChoclate_EFCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.  
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940  
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region Connection String  
            services.AddDbContext<DatabaseContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));
            #endregion

            services.AddScoped<Query>();
            services.AddScoped<Mutation>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddGraphQLServer().AddType<GraphQLTypes>()
                                       .AddQueryType<Query>()
                                       .AddMutationType<Mutation>()
                                       .AddProjections()
                                       .AddFiltering()
                                       .AddSorting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePlayground(new PlaygroundOptions
                {
                    QueryPath = "/api",
                    Path = "/playground"
                });
            }

            app.UseGraphQL("/api");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await Task.Run(() => context.Response.Redirect("/playground"));
                });
            });
        }
    }
}
