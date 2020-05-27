using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
namespace Ruag.DTO
{
    public class OAuthDTO
    {
        [Newtonsoft.Json.JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        [Newtonsoft.Json.JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        [Newtonsoft.Json.JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
