using AruCareApi.ViewModels;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using System;
using System.Web.Http;

namespace AruCareApi.Controllers
{
    public class ValidateController : ApiController
    {
        TokenValidationHandler tvh;
        private IConfiguration configuration;
        public ValidateController(IConfiguration iconfig)
        {
            configuration = iconfig;
            tvh = new TokenValidationHandler(iconfig);
        }       

        [Route("Validate/Valid")]
        [HttpGet]
        public Response Valid(string token)
        {
            Response rp = new Response();
            try
            {
                JWTService jt = new JWTService();
                DateTime dt = jt.GetExpiryTimestamp(token);

                if (DateTime.Now > dt)
                {
                    rp.RespondeCode = 2;
                    rp.Message = "Expired";
                }
                else
                {
                    rp.RespondeCode = 1;
                    rp.Message = "Valid";
                }

                return rp;
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }
            return rp;
        }

        [Route("Validate/Generate")]
        [HttpGet]
        public Response Generate()
        {
            Response rp = new Response();
            try
            {
                string newtoken = tvh.GenerateToken("AruCare");
                rp.RespondeCode = 1;
                rp.Object = newtoken;
                rp.Message = "Generated";
                return rp;
            }
            catch (Exception ex)
            {
                rp.RespondeCode = 3;
                rp.Message = ex.Message;
            }
            return rp;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
    public class JWTService
    {
        private IJsonSerializer _serializer = new JsonNetSerializer();
        private IDateTimeProvider _provider = new UtcDateTimeProvider();
        private IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

        public class JwtToken
        {
            public long exp { get; set; }
        }
        public DateTime GetExpiryTimestamp(string accessToken)
        {
            try
            {
                IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                var token = decoder.DecodeToObject<JwtToken>(accessToken);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                return dateTimeOffset.LocalDateTime;
            }
            catch (TokenExpiredException)
            {
                return DateTime.MinValue;
            }
            catch (SignatureVerificationException)
            {
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                // ... remember to handle the generic exception ...
                return DateTime.MinValue;
            }
        }
    }
}