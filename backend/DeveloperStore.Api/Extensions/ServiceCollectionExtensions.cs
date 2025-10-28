using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Infrastructure.Repositories;

namespace DeveloperStore.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<CreateSaleUseCase>();
            services.AddScoped<GetSaleByIdUseCase>();
            services.AddScoped<GetAllSalesUseCase>();
            services.AddScoped<UpdateSaleUseCase>();
            services.AddScoped<CancelSaleUseCase>();

            services.AddScoped<ISaleRepository, SaleRepository>();

            return services;
        }
    }
}