using BehtashShirzad.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BehtashShirzad.Controllers.Attrubutes
{

    public class JwtAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public JwtAuthorization(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("Token", out var token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetValidationParameters();
                     
                    // Validate token. This throws an exception if the token is invalid.
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                    var claimsIdentity = context.HttpContext.User.Identity as ClaimsIdentity;
                    // Extract roles from the token claims
                    var roles = principal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                    // Check if the user has any of the required role(s)
                    if (roles.Contains("admin")) { return; }
                    if (_roles.Any(role => roles.Contains(role)))
                    {
                        return; // User has the required role
                    }
                    else
                    {
                        context.Result = new ForbidResult(); // User does not have the required role
                        return;
                    }
                }
                catch (Exception)
                {
                    // Token validation failed
                    context.HttpContext.Response.Cookies.Delete("Token");
                   
                    context.Result = new RedirectToActionResult("Login", "Authentication",null);
                   
                    return;
                }
            } 
            context.Result = new RedirectToActionResult("Login", "Authentication", null);

            return;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Infrastructure.SecreteKeyJWT),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero // Optional: prevent clock skew issues
            };
        }
    }

}
