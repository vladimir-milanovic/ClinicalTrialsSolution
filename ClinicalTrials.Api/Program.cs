using ClinicalTrials.Api.Extensions;
using ClinicalTrials.Api.Validators;
using ClinicalTrials.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Serilog;

public class Program
{
    protected Program() { }

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo
            .Console()
            // .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting web application...");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            if (builder.Environment.EnvironmentName != "Testing")
            {
                builder.Services.ConfigurePersistence(builder.Configuration);
            }
            
            builder.Services.AddApplicationDependencies();
            builder.Services.AddServiceCollection();

            builder.Services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UploadRequestValidator>());

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var serviceScope = app.Services.CreateScope();
            var dataContext = serviceScope.ServiceProvider.GetService<ClinicalTrialsDbContext>();
            dataContext?.Database.EnsureCreated();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.DocumentTitle = "Clinical Trials API";
                    options.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}


