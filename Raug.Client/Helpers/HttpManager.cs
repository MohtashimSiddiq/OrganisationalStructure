using Ruag.Client.Helpers.Enums;
using Ruag.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ruag.Common;
using Ruag.Common.Enums;


namespace Ruag.Client.Helpers
{
    public class HttpManager
    {
        private HttpClient _httpClient;
        private static HttpManager _instance;
       
        public static HttpManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HttpManager();
                }
                return _instance;
            }
        }
        private HttpManager()
        {
            _httpClient = new HttpClient();
            var server = ConfigurationReader.Instance.GetAppSetting(AppSettingKeys.ServerAddress).ToString();
            _httpClient.BaseAddress = new Uri(server, UriKind.RelativeOrAbsolute);
        }

        public dynamic Post(HttpContent content, string apiPath)
        {
           
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            dynamic returnObj = null;
            try
            {
                var response = _httpClient.PostAsync(apiPath, content).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    throw new Exception(response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return returnObj;
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public dynamic Get(string urlEncoded,string apiPath)
        {

            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            dynamic returnObj = null;
            try
            {
                string uri = string.Empty;
                if (!string.IsNullOrEmpty(urlEncoded))
                {
                    uri = string.Format("{0}?{1}", apiPath, urlEncoded);
                }
                else
                {
                    uri = apiPath;
                }
                var response = _httpClient.GetAsync(uri).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    throw new Exception(response.Content.ToString());
                }
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());
                return returnObj;
            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }

        public void Login()
        {
            AppLogger.Instance.LogBegin(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            try
            {
                List<KeyValuePair<string, string>> formUrlParams = new List<KeyValuePair<string, string>>();
                formUrlParams.Add(new KeyValuePair<string, string>("username", "SuperUser"));
                formUrlParams.Add(new KeyValuePair<string, string>("password", "P@ssw0rd"));
                formUrlParams.Add(new KeyValuePair<string, string>("grant_type", "password"));
                FormUrlEncodedContent urlContent = new FormUrlEncodedContent(formUrlParams);
                HttpResponseMessage message = _httpClient.PostAsync(APIPaths.OAuth, urlContent).Result;
                var oAuth = Newtonsoft.Json.JsonConvert.DeserializeObject<OAuthDTO>(message.Content.ReadAsStringAsync().Result);
                if (oAuth != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", "bearer: " + oAuth.AccessToken);
                }
            }
            catch (Exception ex)
            {
                AppLogger.Instance.Log(eLogType.Error, ex.ToString());

            }
            finally
            {
                AppLogger.Instance.LogEnd(this.GetType().Name, System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
        }
    }
}
