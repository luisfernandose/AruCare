using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AruCareApi.Controllers
{
    public class TokenValidationHandler
    {
        private IConfiguration configuration;
        public TokenValidationHandler(IConfiguration iconfig)
        {
            configuration = iconfig;
        }
        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }


        public bool validatetoken(string token)
        {
            try
            {
                var secretKey = configuration.GetSection("token").GetSection("JWT_SECRET_KEY").Value;
                var audienceToken = configuration.GetSection("token").GetSection("JWT_AUDIENCE_TOKEN").Value;
                var issuerToken = configuration.GetSection("token").GetSection("JWT_ISSUER_TOKEN").Value;
                var expireTime = configuration.GetSection("token").GetSection("JWT_EXPIRE_MINUTES").Value;
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                IdentityModelEventSource.ShowPII = true;

                SecurityToken securityToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };

                Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateToken(string login)
        {
            // appsetting for Token JWT
            var secretKey = configuration.GetSection("token").GetSection("JWT_SECRET_KEY").Value;
            var audienceToken = configuration.GetSection("token").GetSection("JWT_AUDIENCE_TOKEN").Value;
            var issuerToken = configuration.GetSection("token").GetSection("JWT_ISSUER_TOKEN").Value;
            var expireTime = configuration.GetSection("token").GetSection("JWT_EXPIRE_MINUTES").Value;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, login)
            });

            // create token to the user
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            return jwtTokenString;
            //return Encrypt(jwtTokenString);
        }
        public static DateTime ConvertTimespan(uint timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp);
        }
        internal class TokenValidateHandler : DelegatingHandler
        {
            private IConfiguration configuration;
            public TokenValidateHandler(IConfiguration iconfig)
            {
                configuration = iconfig;
            }
            private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
            {
                token = null;
                IEnumerable<string> authzHeaders;
                if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
                {
                    return false;
                }
                var bearerToken = authzHeaders.ElementAt(0);
                //con esta linea se desencripta si es que colocamos el token encriptado con el MD
                //token = Decrypt(bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken);
                token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                return true;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                string token;
                HttpResponseMessage st = new HttpResponseMessage();

                // determine whether a jwt exists or not
                if (!TryRetrieveToken(request, out token))
                {
                    st.StatusCode = HttpStatusCode.Unauthorized;
                    st.Headers.Add("Message", "Unauthorized");
                    return Task<HttpResponseMessage>.Factory.StartNew(() => st);
                }

                try
                {
                    var secretKey = configuration.GetSection("token").GetSection("JWT_SECRET_KEY").Value;
                    var audienceToken = configuration.GetSection("token").GetSection("JWT_AUDIENCE_TOKEN").Value;
                    var issuerToken = configuration.GetSection("token").GetSection("JWT_ISSUER_TOKEN").Value;
                    var expireTime = configuration.GetSection("token").GetSection("JWT_EXPIRE_MINUTES").Value;
                    var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
                    IdentityModelEventSource.ShowPII = true;

                    SecurityToken securityToken;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        ValidAudience = audienceToken,
                        ValidIssuer = issuerToken,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        //LifetimeValidator = this.LifetimeValidator,
                        IssuerSigningKey = securityKey
                    };

                    // Extract and assign Current Principal and user
                    Thread.CurrentPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                    //HttpContext.Current.User = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                    return base.SendAsync(request, cancellationToken);
                }
                catch (SecurityTokenValidationException ex)
                {
                    if (ex.ToString().Contains("Lifetime validation failed"))
                    {
                        ClaimsPrincipal cp = getprincipalnotime(token);
                        st.StatusCode = HttpStatusCode.PreconditionFailed;
                        st.Headers.Add("Message", "Session End");
                        //autil.MensajeRetorno(ref rp, 1, string.Empty, null, HttpStatusCode.Redirect);
                    }
                    else
                    {
                        st.StatusCode = HttpStatusCode.Unauthorized;
                        st.Headers.Add("Message", "Unauthorized");
                        //autil.MensajeRetorno(ref rp, 1, string.Empty, null, HttpStatusCode.Unauthorized);
                    }

                }
                catch (Exception ex)
                {
                    st.StatusCode = HttpStatusCode.InternalServerError;
                    st.Headers.Add("Message", "General Error");
                    //autil.MensajeRetorno(ref rp, 4, string.Empty, null, HttpStatusCode.InternalServerError);
                }

                return Task<HttpResponseMessage>.Factory.StartNew(() => st);
            }

            public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
            {
                if (expires != null)
                {
                    if (DateTime.UtcNow < expires) return true;
                }
                return false;
            }

            private ClaimsPrincipal getprincipalnotime(string token)
            {
                var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
                var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
                var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                SecurityToken securityToken;
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey
                };
                return tokenHandler.ValidateToken(token, validationParameters, out securityToken);

            }
        }
    }
}