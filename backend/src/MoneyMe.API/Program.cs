using MeoneyMe.Application;
using MoneyMe.Api.Endpoints;
using MeoneyMe.Application.Mappings;
using System.Text.Json.Serialization;
using MoneyMe.Contracts.Response;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services
            .AddApplication()
            .AddInfrastructure();


builder.Services.AddSingleton<IMoneyMeEndpoints, MoneyMeEndpoints>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


builder.Services.AddCors(cors => {
    cors.AddPolicy("AllowAll",
        policy =>
        {
             policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    );
});


var configurations = builder.Configuration;

builder.Services.AddScoped<IDbConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();  
    var envConn = Environment.GetEnvironmentVariable("CONNECTION_STRING");

    var connectionString = envConn ?? configuration.GetSection("Connections").GetValue<string>("ConnectionString");
    return new SqlConnection(connectionString);
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7170);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
MoneyMeMapsterConfig.RegisterMappings();

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();
app.UseCors("AllowAll");

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

var moneyMeApiEndpoints = app.Services.GetServices<IMoneyMeEndpoints>();

foreach(var api in moneyMeApiEndpoints)
{
    api.RegisterEndpoints(app);
}


app.Run();
