﻿using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using Ruag.Common;
using Ruag.Common.Enums;
using Ruag.Data;
using Ruag_WebAPI.Providers;

[assembly: OwinStartup(typeof(Ruag_WebAPI.Startup))]
namespace Ruag_WebAPI
{
     
    public class Startup
    {
      
        public void Configuration(IAppBuilder app)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                HttpConfiguration httpConfig = new HttpConfiguration();
                ConfigureOAuthTokenGeneration(app);
                ConfigureOAuthTokenConsumption(app);
                ConfigureWebApi(httpConfig);
                app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                app.UseWebApi(httpConfig);
            }
            catch (Exception ex)
            {

                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
            }
           
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(AppDBContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
           
          
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost/")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                var issuer = "http://localhost";
                string audienceId = ConfigurationReader.Instance.GetAppSetting(AppSettingKeys.AudienceId);
                byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationReader.Instance.GetAppSetting(AppSettingKeys.AudienceSecret));

                // Api controllers with an [Authorize] attribute will be validated with JWT
                app.UseJwtBearerAuthentication(
                    new JwtBearerAuthenticationOptions
                    {
                        AuthenticationMode = AuthenticationMode.Active,
                        AllowedAudiences = new[] { audienceId },
                        IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                        {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                        }
                    });
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
            }
           
             AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
        }
    }
}