using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Services;
using ClusterBackendAPI.Services.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;  // <-- Correct namespace
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

        //var _jwtLifespans = int.Parse(configuration["Jwt:Lifespan"]);

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

        // Add Authentication
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;  // Set to false if testing locally
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7142",  // Ensure this matches the token issuer
            ValidAudience = "https://localhost:7142", // Ensure this matches the token audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });

        // Add Swagger
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
