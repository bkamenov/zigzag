using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess;
using DataAccess.Repositories;
using ApiAccess.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();

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

// Add authentication
var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
if (jwtSettings == null)
  throw new InvalidProgramException("JwtSettings are missing.");

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
builder.Services.AddAuthorization();

// Register services and repositories
builder.Services.AddSingleton<IJwtTokenRepository, JwtTokenRepository>(); // Singleton to keep the token cleanup service running
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add controllers
builder.Services.AddControllers();

// Run backgroud tasks
builder.Services.AddHostedService<TokenCleanupService>();

// Configure Kestrel to use configuration file
builder.WebHost.ConfigureKestrel((context, options) =>
{
  // Use endpoints defined in appsettings.json or environment-specific files
  options.Configure(context.Configuration.GetSection("Kestrel"));

  // Load and apply certificate for HTTPS endpoints
  var httpsConfig = context.Configuration.GetSection("Kestrel:Endpoints:Https:Certificate");
  if (httpsConfig.Exists())
  {
    var certPath = httpsConfig["Path"];
    var keyPath = httpsConfig["KeyPath"];
    var certificate = SSLCertificateHelper.LoadCertificate(certPath!, keyPath!);
    options.ListenAnyIP(443, listenOptions => listenOptions.UseHttps(certificate));
  }
});

var app = builder.Build();

app.UseMiddleware<TokenVerificationMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
