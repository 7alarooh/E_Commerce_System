using Microsoft.EntityFrameworkCore;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using E_CommerceSystem.Repositories;
using E_CommerceSystem.Helpers;

namespace E_CommerceSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Scoped Repositories and Services
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderProductsRepository, OrderProductsRepository>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderProductsService, OrderProductsService>();
            // Add AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Configure JWT Authentication - Only ONE registration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForJWT12345")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Add Authorization
            builder.Services.AddAuthorization();

            // Configure DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Controllers and Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Ensure correct middleware order
            app.UseAuthentication(); // Add this before Authorization
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
