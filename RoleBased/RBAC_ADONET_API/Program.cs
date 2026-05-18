
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using RBAC_ADONET_API.Repositories;
using RBAC_ADONET_API.Services;
using System.Text;

namespace RBAC_ADONET_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

       

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,

                ValidateAudience = true,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,

                ValidIssuer = "myapp",

                ValidAudience = "myapp",

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            "THIS_IS_MY_SECRET_KEY_123"))
            };
    });
           
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1",
            //        new OpenApiInfo
            //        {
            //            Title = "RBAC API",
            //            Version = "v1"
            //        });

            //    options.AddSecurityDefinition("Bearer",
            //        new OpenApiSecurityScheme
            //        {
            //            Name = "Authorization",

            //            Type = SecuritySchemeType.Http,

            //            Scheme = "bearer",

            //            BearerFormat = "JWT",

            //            In = ParameterLocation.Header,

            //            Description =
            //                "Enter JWT Token"
            //        });

            //    options.AddSecurityRequirement(
            //        new OpenApiSecurityRequirement
            //        {
            //{
            //    new OpenApiSecurityScheme
            //    {
            //        Reference =
            //            new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //    },

            //    Array.Empty<string>()
            //}
            //        });
            //});


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
               
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}
