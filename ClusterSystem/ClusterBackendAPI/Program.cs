using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Services.Repo;
using ClusterBackendAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        var builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cluster API", Version = "v1" });
        });

        var app = builder.Build();

        app.UseSwagger();

        // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.)
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cluster API v1");
            c.RoutePrefix = string.Empty;  // Optional: sets Swagger UI at the root (http://localhost:5000)
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