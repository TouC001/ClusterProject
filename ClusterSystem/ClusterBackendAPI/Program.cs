using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Services.Repo;
using ClusterBackendAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

internal class Program
{
    private static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

        // Load environment variable for JWT Secret
        var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
        if (string.IsNullOrEmpty(jwtSecret))
        {
            throw new Exception("JWT Secret is missing.");
        }

        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        // Override JWT secret in the configuration
        builder.Configuration["Jwt:Secret"] = jwtSecret;

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("https://localhost:7142",
                                                      "http://localhost:5155");
                              });
        });

        // Add services to the container.
        builder.Services.AddDbContext<ClusterDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddScoped<Repository>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<UserAthenticationService>();
        builder.Services.AddScoped<IPasswordHasher<ClusterBackendAPI.Models.User>, PasswordHasher<ClusterBackendAPI.Models.User>>();


        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cluster API", Version = "v1" });
        });

        var app = builder.Build();

        app.UseSwagger();

        // Enable middleware to serve Swagger UI.
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cluster API v1");
            c.RoutePrefix = string.Empty; 
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
        }

        app.UseHttpsRedirection();

        app.UseCors(MyAllowSpecificOrigins);

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}