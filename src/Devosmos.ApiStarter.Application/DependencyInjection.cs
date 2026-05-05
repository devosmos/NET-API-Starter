using Devosmos.ApiStarter.Application.Abstractions.Operations;
using Devosmos.ApiStarter.Application.Common.Operations;
using Devosmos.ApiStarter.Application.System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Devosmos.ApiStarter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        services.AddScoped<IOperationExecutor, OperationExecutor>();
        services.AddScoped<IOperationHandler<GetApiInfoRequest, ApiInfoResponse>, GetApiInfoOperation>();

        return services;
    }
}
