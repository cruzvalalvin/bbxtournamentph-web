using BBXTournament.Application.Interfaces.Auth;
using BBXTournament.Application.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace BBXTournament.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}

// Made with Bob
