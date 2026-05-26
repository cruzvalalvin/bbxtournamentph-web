using Microsoft.Extensions.DependencyInjection;

namespace BBXTournament.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Add MediatR for CQRS pattern (optional, can be added later)
        // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        
        // Add application services here
        // services.AddScoped<ITournamentService, TournamentService>();
        
        return services;
    }
}

// Made with Bob
