using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Model.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class JWTHelp
    {
        public string secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
        public bool getValue(string token)
        {
            bool result = true;
            T_SysUser user = new T_SysUser();
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.Decode(token, secret, verify: true);
                user = JsonConvert.DeserializeObject<T_SysUser>(json);
                
            }
            catch (TokenExpiredException)
            {
                result = false;
            }
            catch (SignatureVerificationException)
            {
                result = false;
            }
            catch (Exception )
            {
                result = false;
            }
            return result;
        }
        public string getToken(string name, string roleid)
        {
        
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);
            var payload = new Dictionary<string, object>
            {
               {"Name", name},
               {"exp",secondsSinceEpoch+60*60*24 },
               {"Password",roleid }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);
            return token;
        }
    }
}
