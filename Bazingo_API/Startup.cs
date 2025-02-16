using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bazingo_Application.Interfaces;
using Bazingo_Application.Services;
using Bazingo_Core.Interfaces;
using Bazingo_Core.Services;
using Bazingo_Infrastructure.Repositories;
using Bazingo_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bazingo_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Register Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Register Review Services
            services.AddScoped<IReviewService, ReviewCoreService>();
            services.AddScoped<ReviewApplicationService>();

            // Register User Services
            services.AddScoped<IUserService, Bazingo_Core.Services.UserCoreService>();
            services.AddScoped<IUserApplicationService, UserApplicationService>();

            // Register Search Services
            services.AddScoped<ISearchService, SearchService>();

            // Register Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
