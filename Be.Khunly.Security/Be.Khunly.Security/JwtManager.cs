using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Be.Khunly.Security
{
    public class JwtManager(JwtSecurityTokenHandler _handler, JwtManager.JwtConfig _config)
    {
        public class JwtConfig
        {
            public required string Issuer { get; set; }
            public required string Audience { get; set; }
            public required string Signature { get; set; }
            public required int Duration { get; set; }
        }

        public string CreateToken(string username, string identifier, string email, params string[] roles)
        {
            JwtSecurityToken t = new JwtSecurityToken(
                _config.Issuer,
                _config.Audience,
                CreatePayload(username, identifier, email, roles),
                DateTime.Now,
                _config.Duration == 0 ? null : DateTime.Now.AddSeconds(_config.Duration),
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Signature))
                    , SecurityAlgorithms.HmacSha256)
            );
            return _handler.WriteToken(t);
        }

        private IEnumerable<Claim> CreatePayload(string username, string identifier, string email, string[] roles)
        {
            yield return new Claim(ClaimTypes.Name, username);
            yield return new Claim(ClaimTypes.NameIdentifier, identifier);
            yield return new Claim(ClaimTypes.Email, email);
            foreach (string role in roles)
            {
                yield return new Claim(ClaimTypes.Role, role);
            }
        }
    }
}
