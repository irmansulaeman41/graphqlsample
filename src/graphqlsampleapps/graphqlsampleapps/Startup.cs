using graphqlsampleapps.DataAccess;
using graphqlsampleapps.GraphQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace graphqlsampleapps
{
    public class Startup
    {

        private IConfiguration _configuration;

        public Startup(IConfiguration c)
        {
            _configuration = c;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = _configuration.GetConnectionString("AppConnStr");

            services.AddPooledDbContextFactory<AppDbContext>(opt => opt.UseSqlServer(sqlConnectionString));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConnectionString));

            services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddFiltering()
                .AddSorting()
                .AddInMemorySubscriptions();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
