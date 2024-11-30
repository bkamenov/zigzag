using GraphQL.Types;
using GraphQL.MicrosoftDI;
using GraphQL;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GraphQL.Validation;
using GraphQL.Server.Transports.AspNetCore;
using ApiAccess.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Load configuration settings
var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

// Add MongoDB configuration
var mongoSettings = builder.Configuration.GetSection("MongoDB");
var databaseContext = new DatabaseContext(
    mongoSettings["ConnectionString"]!,
    mongoSettings["Database"]!
);
builder.Services.AddSingleton(databaseContext.GetDatabase());
builder.Services.AddScoped<ICryptoRepository, CryptoRepository>();

builder.Services.AddScoped<IJwtTokenRepository, JwtTokenRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
if (jwtSettings == null)
  throw new InvalidProgramException("JwtSettings are missing.");

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero // Remove delay in token expiration
      };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("RegularUser", policy => policy.RequireRole("user"));
  options.AddPolicy("AdminUser", policy => policy.RequireRole("admin"));
});

// Add GraphQL services
builder.Services.AddScoped<CryptoQuery>();
builder.Services.AddScoped<CryptoMutation>();
builder.Services.AddSingleton<ISchema, CryptoSchema>(services => new CryptoSchema(new SelfActivatingServiceProvider(services)));

// ...just no comment how authorization is added to GQL in .NET Core
builder.Services.AddTransient<IValidationRule, AuthorizationValidationRule>();
builder.Services.AddGraphQL(builder =>
{
  builder.AddSystemTextJson();
  builder.AddValidationRule<AuthorizationValidationRule>(); // Тъпо като гъбено семе! Кво ви пречеше в шибаната фиункция AddGraphQL да се интегрирате с дотнетската авторизация или да турите едно флагче. Тъпанари...
  builder.AddUserContextBuilder(httpContext => new GraphQLUserContext(httpContext.User)); // Що треби я да се мъча, бе, уйови глави? То е ясно, че трябва да ги има принсипалите, ама що, говеда, не си ги прихващате сами, а трябва като малоумен да ви чета пробития сорс код и да гадая на боб защо авторизацията не работи. Поне един ексепшън да бехте фърлили като се вика Ауторизе на тъпите ви рикуести.
});

// Configure Kestrel to use configuration file
builder.WebHost.ConfigureKestrel((context, options) =>
{
  // Use endpoints defined in appsettings.json or environment-specific files
  options.Configure(context.Configuration.GetSection("Kestrel"));

  // Load and apply certificate for HTTPS endpoints
  var httpsConfig = context.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");
  if (httpsConfig.Exists())
  {
    var certPath = httpsConfig["Path"] ?? string.Empty;
    var keyPath = httpsConfig["KeyPath"] ?? string.Empty;
    var certificate = SSLCertificateHelper.LoadCertificate(certPath, keyPath);
    options.ListenAnyIP(443, listenOptions => listenOptions.UseHttps(certificate));
  }
});

var app = builder.Build();

// Middleware configuration
app.UseMiddleware<TokenVerificationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseGraphQL<ISchema>("/graphql");
app.UseGraphQLPlayground(path: "/playground");

app.Run();

