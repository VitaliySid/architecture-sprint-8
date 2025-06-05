using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.IdentityModel.Tokens;
using report_api.Reports;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;

var authServerUrl = Environment.GetEnvironmentVariable("WEB_API_KEYCLOAK_URL") ?? configuration["Keycloak:auth-server-url"];
var realm = Environment.GetEnvironmentVariable("WEB_API_KEYCLOAK_REALM") ?? configuration["Keycloak:realm"];
var clientId = Environment.GetEnvironmentVariable("WEB_API_KEYCLOAK_CLIENT_ID") ?? configuration["Keycloak:resource"];

services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.MaxDepth = 18;
    options.SerializerOptions.AllowTrailingCommas = true;
    options.SerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    options.SerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
services.AddKeycloakWebApiAuthentication(configuration, (x) =>
{
    x.Authority = $"{authServerUrl}/realms/{realm}";
    x.RequireHttpsMetadata = false;
    x.Audience = clientId;

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

services.AddAuthorization(o =>
{
    o.AddPolicy("prothetic_user", b =>
    {
        b.RequireRealmRoles("prothetic_user");
    });
    o.AddPolicy("administrator", b =>
    {
        b.RequireRealmRoles("administrator");
    });
});

services.AddCors(options =>
{
    options.AddPolicy("policy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000"
                )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

services.AddKeycloakAuthorization(configuration);
services.AddAuthorization();
try
{
    var app = builder.Build();

    var reportsApi = app.MapGroup("/reports");
    reportsApi.MapGet("/", () =>
    {
        var result = JsonSerializer.Serialize(FakeReports.Generate(), new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
        app.Logger.LogInformation(result);

        return result;
    })
        .RequireAuthorization("prothetic_user");

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("policy");

    app.Logger.LogInformation($"Keycloak server {authServerUrl}");
    app.Logger.LogInformation($"Realm {realm}");
    app.Logger.LogInformation($"ClientId {clientId}");

    app.Run();
}
catch (Exception ex) { }


