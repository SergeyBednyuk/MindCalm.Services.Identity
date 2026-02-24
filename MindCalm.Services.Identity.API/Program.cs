using MindCalm.Services.Identity.Core;
using MindCalm.Services.Identity.Core.Features.Auth.Login;
using MindCalm.Services.Identity.Infrastructure.Extensions;

namespace MindCalm.Services.Identity.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddCore(builder.Configuration);
        
        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(GuestLoginCommand).Assembly));
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}