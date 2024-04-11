using ElliotStore.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BehtashShirzad.Controllers.Attrubutes
{
    public class JwtAuthorization : Attribute, IAsyncActionFilter
    {

       

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Cookies.TryGetValue("Token", out var token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var validationParameters = GetValidationParameters();

                    // Validate token. This throws an exception if the token is invalid.
                    var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                    // Optionally set the HttpContext.User to the principal,
                    // which represents the authenticated user.
                    context.HttpContext.User = principal;
                    context.HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);
                    await next();
                    return;
                }
                catch (Exception)
                {
                    // Token validation failed
                    context.HttpContext.Response.Cookies.Delete("Token");
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            // No token found
            context.Result = new UnauthorizedResult();
      
            context.Result = new RedirectToActionResult("Login", "Authentication", null);
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
