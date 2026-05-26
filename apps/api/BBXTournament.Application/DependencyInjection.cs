using BBXTournament.Application.Interfaces.Auth;
using BBXTournament.Application.Interfaces.Communities;
using BBXTournament.Application.Services.Auth;
using BBXTournament.Application.Services.Communities;
using Microsoft.Extensions.DependencyInjection;

namespace BBXTournament.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICommunityService, CommunityService>();

        return services;
    }
}

// Made with Bob
