using Application.Interfaces;
using Presentation;
using Infrastructure.Mapping;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi Layanan
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.ConfigurePersistenceServices(builder.Configuration);
var modelEDM = OdataServiceRegistration.GetEdmModel();
builder.Services.ConfigureOdata(modelEDM);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

// Middleware
app.ConfigureExceptionHandling();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Odata API w/ Clean Architecture");
    options.DefaultModelsExpandDepth(-1);
    options.EnableTryItOutByDefault();

    if (!app.Environment.IsDevelopment())
    {
        options.RoutePrefix = string.Empty;
    }
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.ConfigureODataMiddleware();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Migrate database
await builder.Services.BuildServiceProvider().ApplyMigrationsAsync();

app.Run();