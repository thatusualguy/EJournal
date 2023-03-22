using EJournalGrpcService.Services;

using EJournal.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddDbContext<EJournalContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PosgresConnection")));
builder.Services.AddScoped<IExcelReaderService, ExcelReaderService>();

// Set up JWT Token authentication using custom ValidationParameters
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(auth =>
{
	auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	var config = builder.Configuration.GetSection("JwtToken");
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidIssuer = config["Issuer"] ?? throw new ArgumentNullException("JwtToken::Issuer"),
		ValidateAudience = true,
		ValidAudience = config["Audience"] ?? throw new ArgumentNullException("JwtToken::Audience"),
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
						config["SigningKey"] ?? throw new ArgumentNullException("JwtToken::SigningKey")))
	};
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

// Allows reflection for debugging
IWebHostEnvironment env = app.Environment;
if (env.IsDevelopment())
	app.MapGrpcReflectionService();


app.Run();
