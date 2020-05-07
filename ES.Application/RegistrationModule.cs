﻿using ES.Application.Services;
using ES.Application.Services.Interfaces;
using ES.Infrastructure.ElasticSearch;
using ES.Infrastructure.ElasticSearch.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ES.Application
{
    public static class RegistrationModule
    {
        public static IServiceCollection RegisterApplicationModule(this IServiceCollection services)
        {
            return services
                .AddScoped<ISearchService, SearchService>()
                .AddScoped<IPropertyService, PropertyService>()
                .AddScoped<IManagementService, ManagementService>();
        }
    }
}
