using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using MediatR;
using Telegram.Application.Contracts;
using System.Configuration;

namespace Telegram.Application
{
   public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
          
           
            services.AddMediatR(Assembly.GetExecutingAssembly());
           
        }
    }
}
