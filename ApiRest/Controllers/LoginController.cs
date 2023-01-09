using ApiRest.DTO;
using ApiRest.Model;
using ApiRest.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiRest.Controllers
{
    [Route("Login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IRepositoryUser repo;
        private readonly ILogger logger;

        public LoginController(IConfiguration config, IRepositoryUser repo, ILogger<LoginController> logger)
        {
            this.config = config;
            this.repo = repo;
            this.logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DTOUser>> Login(LoginAPI api)
        {
            User? user = await Authenticate(api);

            if (user == null || user.toDTO() == null)
            {
                logger.LogError("Invalid credentials for " + api.Email);
                return null;
            }

            return GenerateJWT(user).toDTO();
        }

        protected async Task<User?> Authenticate(LoginAPI api)
        {
            User? user = await repo.LoginAsync(api);

            return user;
        }

        private User GenerateJWT(User user)
        {
            // Prepare Jason Web Token Header
            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JWT:Secret"])
                );
            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var header = new JwtHeader( signingCredentials );

            // Prepare Jason Web Token Claims
            var claims = new[]
            {
                new Claim("Name", user.Name),
                new Claim("Email", user.Email),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email),
            };

            // Prepare Jason Web Token Payload
            var payload = new JwtPayload(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1)
                );

            // Final preparation of Jason Web Token
            var token = new JwtSecurityToken(
                header,
                payload
                );
            user.Token = new JwtSecurityTokenHandler().WriteToken( token );


            return user;
        }
    }
}
