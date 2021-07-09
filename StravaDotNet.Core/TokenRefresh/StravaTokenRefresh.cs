using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Strava.TokenRefresh
{
    public static class StravaTokenRefresh
    {
        public const string TokenEndpoint = "https://www.strava.com/oauth/token";

        public static async Task<Token> RefreshToken(Token currentToken, string clientId, string clientSecret)
        {

            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;

            //string getUrl = string.Format("{0}?client_id={1}&client_secret={2}&grant_type=refresh_token&refresh_token={3}",
            //StravaAuthenticationHandler.TokenEndpoint,
            //    clientId,
            //    clientSecret,
            //    currentToken.RefreshToken);

            //string json = Http.WebRequest.SendGet(new Uri(getUrl));


            var tokenRequestParameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("refresh_token", currentToken.RefreshToken),
                    new KeyValuePair<string, string>("grant_type", "refresh_token")
                };

            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.PostAsync(StravaTokenRefresh.TokenEndpoint, requestContent);//, Request.CallCancelled);
            response.EnsureSuccessStatusCode();
            string oauthTokenResponse = await response.Content.ReadAsStringAsync();

            JObject oauth2Token = JObject.Parse(oauthTokenResponse);
            string accessToken = oauth2Token["access_token"].Value<string>();
            string refreshToken = oauth2Token["refresh_token"].Value<string>();
            string expiresAt = oauth2Token["expires_at"].Value<string>();
            string expiresIn = oauth2Token["expires_in"].Value<string>();


            Token newToken = new Token();
            newToken.AccessToken = accessToken;
            newToken.RefreshToken = refreshToken;
            newToken.ExpiresAt = Convert.ToInt32(expiresAt);
            newToken.ExpiresIn = Convert.ToInt32(expiresIn);

            return newToken;
        }


    }
}
