using EJournalGrpcService.Security;

using Grpc.Core;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace EJournalGrpcService.Services
{
	[Authorize]
	public class GreeterService : Greeter.GreeterBase
	{
		private readonly ILogger<GreeterService> _logger;
		private readonly IConfiguration _configuration;

		public GreeterService(ILogger<GreeterService> logger, IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;


		}

		[Authorize(Roles = "Student")]
		public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
		{
			return Task.FromResult(new HelloReply
			{
				Message = $"Hello {context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)}: " + context.GetHttpContext().User.Claims.Select(x => x.ToString()).Aggregate((acc, x) => acc += x + ' ')
			});
		}

		[AllowAnonymous]
		public override Task<LoginReply> Login(LoginRequest request, ServerCallContext context)
		{
			var jwt_config = _configuration.GetSection("JwtToken");

			string uniqueKey = jwt_config["SigningKey"];
			string issuer = jwt_config["Issuer"];
			string audience = jwt_config["Audience"];
			string config_val = jwt_config["Expiration"];
			var expiration = TimeSpan.FromSeconds(int.Parse(config_val));

			Claim[] claims = { new Claim(ClaimTypes.Role, "Student") };
			return Task.FromResult(new LoginReply
			{
				Token = JwtHelper.GetJwtTokenString(
					request.Name,
					uniqueKey,
					issuer,
					audience,
					expiration,
					claims)
			});

		}
	}
}