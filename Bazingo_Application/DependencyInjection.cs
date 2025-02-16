    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bazingo_Application.Services;
    using Bazingo_Application.Services.Core;
    using Bazingo_Application.Interfaces;
    using Bazingo_Core.Interfaces;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Bazingo_Application.Services.Identity;

    namespace Bazingo_Application
    {
        public static class DependencyInjection
        {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                services.AddFluentValidationAutoValidation();

                // Register Core Services
                services.AddScoped<Bazingo_Core.Interfaces.IUserService, Bazingo_Application.Services.Core.UserCoreService>();
                services.AddScoped<Bazingo_Core.Interfaces.ICartService, Bazingo_Application.Services.Core.CartCoreService>();
                services.AddScoped<Bazingo_Core.Interfaces.IProductService, Bazingo_Application.Services.Core.ProductCoreService>();
                services.AddScoped<Bazingo_Core.Interfaces.IOrderService, Bazingo_Application.Services.Core.OrderCoreService>();
                services.AddScoped<Bazingo_Core.Interfaces.IReviewService, Bazingo_Application.Services.Core.ReviewCoreService>();
                services.AddScoped<Bazingo_Core.Interfaces.IWishlistService, Bazingo_Application.Services.Core.WishlistCoreService>();

                // Register Application Services
                services.AddScoped<Bazingo_Application.Interfaces.IUserApplicationService, Bazingo_Application.Services.UserApplicationService>();
                services.AddScoped<Bazingo_Application.Interfaces.ICartService, Bazingo_Application.Services.CartApplicationService>();
                services.AddScoped<Bazingo_Application.Interfaces.IProductService, Bazingo_Application.Services.ProductApplicationService>();
                services.AddScoped<Bazingo_Application.Interfaces.IOrderService, Bazingo_Application.Services.OrderApplicationService>();
                services.AddScoped<Bazingo_Application.Interfaces.IWishlistService, Bazingo_Application.Services.WishlistApplicationService>();
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<ITokenService, TokenService>();

                // Register MediatR
                services.AddMediatR(cfg => {
                    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                });

                return services;
            }
        }
    }
