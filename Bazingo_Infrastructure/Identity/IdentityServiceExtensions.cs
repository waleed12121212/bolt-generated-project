    using Bazingo_Core.Settings;
    using Bazingo_Core.Models.Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Bazingo_Core.Entities.Identity;
    using Bazingo_Core;

    namespace Bazingo_Infrastructure.Identity
    {
        public static class IdentityServiceExtensions
        {
            public static IServiceCollection AddIdentityServices(
                this IServiceCollection services ,
                IConfiguration configuration)
            {
                services.AddIdentity<ApplicationUser , IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false; // Set to true if email confirmation is required
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Configure JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.UTF8.GetBytes(configuration[Constants.Configuration.JwtSecretKey]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey ,
                    IssuerSigningKey = new SymmetricSecurityKey(key) ,
                    ValidateIssuer = jwtSettings.ValidateIssuer ,
                    ValidAudience = jwtSettings.ValidAudience ,
                    ValidateLifetime = jwtSettings.ValidateLifetime ,
                    ValidIssuer = jwtSettings.Issuer ,
                    ValidAudience = jwtSettings.Audience ,
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkew)
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired" , "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
