using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenVue.CSharp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeather(this IServiceCollection services)
        {
            // All add weather related DI.
           // services.AddSingleton<Providers.IWeatherProvider, Providers.WeatherProviderFake>();

            return services;
        }
    }
}
