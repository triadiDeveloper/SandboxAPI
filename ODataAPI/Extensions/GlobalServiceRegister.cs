using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.AspNetCore.OData.NewtonsoftJson;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using System.Text;

namespace WebAPI.Extensions
{
    public static class GlobalServiceRegister
    {
        public const string RoutePrefix = "odata";

        public static async Task ApplyMigrationsAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<Infrastructure.CleanDBContext>();
            await dbContext.Database.MigrateAsync();

            #region Run Create or Alter Views

            //await dbContext.RunQueryAsync(View.ViewCollection.ViewName());

            #endregion

            #region Run Create or Alter Store Procedure

            //await dbContext.RunQueryAsync(Persistence.StoreProcedure.ProcedureName);

            #endregion
        }

        public static void ConfigureOdata(this IServiceCollection services, IEdmModel edmModel)
        {
            services.AddControllers()
                .AddOData(options =>
                {
                    // Tambahkan batching jika dibutuhkan
                    var defaultBatchHandler = new DefaultODataBatchHandler
                    {
                        MessageQuotas =
                        {
                            MaxNestingDepth = 2,
                            MaxOperationsPerChangeset = 10,
                            MaxReceivedMessageSize = 100
                        }
                    };

                    // Konfigurasi fitur OData
                    options.Select().Filter().OrderBy().Count().SetMaxTop(null).Expand();

                    // Tambahkan EDM Model dan handler batching
                    options.AddRouteComponents(RoutePrefix, edmModel, defaultBatchHandler);
                })
                .AddODataNewtonsoftJson(); // Opsional, gunakan jika Newtonsoft.Json dibutuhkan
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            // Tambahkan konfigurasi Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<Infrastructure.CleanDBContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Konfigurasi JWT Authentication
            var jwtKey = configuration["Jwt:Key"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtAudience = configuration["Jwt:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web API",
                    Version = "v1",
                    Description = "This is a Odata API w/ Clean Architecture",
                    Contact = new OpenApiContact
                    {
                        Name = "Triadi",
                        Email = "andriyansyah.triadi@interaneka.com",
                        Url = new Uri("https://yourwebsite.com")
                    }
                });

                // Resolve conflicting actions
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        public static void ConfigureODataMiddleware(this IApplicationBuilder app)
        {
            app.UseODataBatching();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public static void ConfigureExceptionHandling(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var error = new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "An unexpected error occurred. Please try again later."
                    };
                    await context.Response.WriteAsJsonAsync(error);
                });
            });
        }
    }
}