using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary.ApiSearch
{
    public class OAuth2
    {
        private string OAuth2Token;
        public string Token_type { get; set; }
        public double Expires_in { get; set; }
        public string Scope { get; set; }

        public string Token => OAuth2Token;

        public async Task GenerateAuth2TokenAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var byteArray = Encoding.ASCII.GetBytes("mhclt_dotNetMovieLibrary:TJUPmRCiyPTepw4QxjFT3ND8MxEID2TX23H7yYFegqdWM33Q");
                    var header = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    client.DefaultRequestHeaders.Authorization = header;
                    client.DefaultRequestHeaders
                          .Accept
                          .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var url = "https://api.mediahound.com/1.3/security/oauth/token";
                    var parameters = new Dictionary<string, string> { { "client_id", "mhclt_dotNetMovieLibrary" }, { "client_secret", "TJUPmRCiyPTepw4QxjFT3ND8MxEID2TX23H7yYFegqdWM33Q" }, { "grant_type", "client_credentials" } };
                    var encodedContent = new FormUrlEncodedContent(parameters);

                    var response = await client.PostAsync(url, encodedContent).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        JObject res = JObject.Parse(responseContent);

                        OAuth2Token = (string)res["access_token"];
                        Token_type = (string)res["token_type"];
                        Expires_in = (double)res["expires_in"];
                        Scope = (string)res["scope"];
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                Debug.WriteLine("Retrieved OAuth2 token");
            }
        }
    }
}
