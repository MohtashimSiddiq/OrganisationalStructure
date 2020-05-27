using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Ruag.Common;
using Ruag.Common.Enums;
using System;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace Ruag_WebAPI.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                if (data == null)
                {
                    throw new ArgumentNullException("data");
                }
                string audienceId = ConfigurationReader.Instance.GetAppSetting(AppSettingKeys.AudienceId);
                string symmetricKeyAsBase64 = ConfigurationReader.Instance.GetAppSetting(AppSettingKeys.AudienceSecret);
                var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
                var signingKey = new HmacSigningCredentials(keyByteArray);
                var issued = data.Properties.IssuedUtc;
                var expires = data.Properties.ExpiresUtc;
                var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.DateTime, expires.Value.DateTime, signingKey);
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.WriteToken(token);
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
                return jwt;
            }
            catch (Exception ex)
            {

                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return null;
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }

        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}