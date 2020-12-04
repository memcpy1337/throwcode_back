using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using throwcode_back.DB_Context;

namespace throwcode_back.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private const string SECRET_KEY = "abcdef33213dfsadsad21edt43t43t34wf432f43f234132f23f23f23f23f43t453t32534";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthController.SECRET_KEY));

        private readonly UserContext _userContext;

        public AuthController(UserContext context)
        {
            _userContext = context;
        }
        [HttpGet("usr={login}/pass={password}", Name = "Get")]
        public IActionResult GetToken(string login, string password)
        {
            var user = _userContext.Users.Where(s => s.Login == login && s.Password == password).FirstOrDefault();
            if (user != null)
            {
                return new ObjectResult(GenerateToken(user.Id, user.Login));
            }
            else
            {
                return BadRequest();
            }
        }

        private string GenerateToken(int id, string username)
        {
            var token = new JwtSecurityToken(
                claims: new Claim[]
                {
                    new Claim(ClaimTypes.UserData, "https://youtu.be/hLCYGWDDpM8"),
                    new Claim(ClaimTypes.SerialNumber, id.ToString()),
                    new Claim(ClaimTypes.Name, username)
                },
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddMinutes(60)).DateTime,
                signingCredentials: new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Encoding.UTF8.GetBytes(AuthController.SECRET_KEY);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }
    }
}

