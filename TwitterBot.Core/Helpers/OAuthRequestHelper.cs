using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace TwitterBot.Core.Helpers
{
    public interface IOAuthRequestHelper
    {
        AuthenticationHeaderValue GetBearerToken();
        string PrepareOAuth1Request(string URL);
    }

    public class OAuthRequestHelper : IOAuthRequestHelper
    {
        private readonly AuthenticationHeaderValue _bearerToken;
        private readonly string _consumerKey;
        private readonly string _accessToken;
        private readonly HMACSHA1 _sigHasher;
        private readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public OAuthRequestHelper(IConfiguration configuration)
        {
            _bearerToken = new AuthenticationHeaderValue("Bearer", $"{configuration["Twitter:BearerToken"]}");
            _consumerKey = configuration["Twitter:ApiKey"];
            _accessToken = configuration["Twitter:AccessToken"];

            var consumerKeySecret = configuration["Twitter:ApiSecret"];
            var accessTokenSecret = configuration["Twitter:AccessTokenSecret"];

            _sigHasher = new HMACSHA1(
                Encoding.UTF8.GetBytes($"{consumerKeySecret}&{accessTokenSecret}")
            );
        }

        public AuthenticationHeaderValue GetBearerToken() => _bearerToken;

        public string PrepareOAuth1Request(string URL)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)(DateTime.UtcNow - _epochUtc).TotalSeconds;

            // Add all the OAuth headers we'll need to use when constructing the hash
            Dictionary<string, string> oAuthData = new Dictionary<string, string>();
            oAuthData.Add("oauth_consumer_key", _consumerKey);
            oAuthData.Add("oauth_signature_method", "HMAC-SHA1");
            oAuthData.Add("oauth_timestamp", timestamp.ToString());
            oAuthData.Add("oauth_nonce", new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture));
            oAuthData.Add("oauth_token", _accessToken);
            oAuthData.Add("oauth_version", "1.0");

            // Generate the OAuth signature and add it to our payload
            oAuthData.Add("oauth_signature", GenerateSignature(URL, oAuthData));

            // Build the OAuth HTTP Header from the data
            return GenerateOAuthHeader(oAuthData);
        }

        private string GenerateSignature(string url, Dictionary<string, string> data)
        {
            var sigString = string.Join(
                "&",
                data
                    .Union(data)
                    .Select(kvp => string.Format("{0}={1}", Uri.EscapeDataString(kvp.Key), Uri.EscapeDataString(kvp.Value)))
                    .OrderBy(s => s)
            );

            var fullSigData = string.Format("{0}&{1}&{2}",
                "POST",
                Uri.EscapeDataString(url),
                Uri.EscapeDataString(sigString.ToString()
                )
            );

            return Convert.ToBase64String(
                _sigHasher.ComputeHash(
                    Encoding.UTF8.GetBytes(fullSigData.ToString())
                )
            );
        }

        private static string GenerateOAuthHeader(Dictionary<string, string> data)
        {
            return string.Format(
                "OAuth {0}",
                string.Join(
                    ", ",
                    data
                        .Where(kvp => kvp.Key.StartsWith("oauth_"))
                        .Select(
                            kvp => string.Format("{0}=\"{1}\"",
                            Uri.EscapeDataString(kvp.Key),
                            Uri.EscapeDataString(kvp.Value)
                            )
                        ).OrderBy(s => s)
                    )
                );
        }
    }
}
