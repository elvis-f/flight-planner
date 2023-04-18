using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using System;

namespace FlightPlanner.Authentication
{
    internal class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                // No authorization header
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            string authHeader = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authHeader))
            {
                // No authorization header value
                return AuthenticateResult.Fail("Missing Authorization Header Value");
            }

            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                // Not a basic authentication header
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            byte[] decodedBytes = Convert.FromBase64String(encodedUsernamePassword);
            string decodedText = Encoding.UTF8.GetString(decodedBytes);

            string[] parts = decodedText.Split(new char[] { ':' }, 2);
            string username = parts[0];
            string password = parts[1];

            if (username == "codelex-admin" && password == "Password123")
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
        }
    }
}