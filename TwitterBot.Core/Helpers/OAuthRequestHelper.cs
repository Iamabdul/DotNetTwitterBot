using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TwitterBot.Core.Helpers
{
    public static class OAuthRequestHelper
    {
        readonly static string _consumerKey;
        readonly static string _consumerKeySecret;
        readonly static string _accessToken;
        readonly static string _accessTokenSecret;
        readonly static HMACSHA1 _sigHasher;
        readonly static DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        static OAuthRequestHelper()
        {
            _consumerKey = ConfigurationManager.AppSettings["Twitter:ApiKey"];
            _consumerKeySecret = ConfigurationManager.AppSettings["Twitter:ApiSecret"];
            _accessToken = ConfigurationManager.AppSettings["Twitter:AccessToken"];
            _accessTokenSecret = ConfigurationManager.AppSettings["Twitter:AccessTokenSecret"];

            _sigHasher = new HMACSHA1(
                Encoding.UTF8.GetBytes($"{_consumerKeySecret}&{_accessTokenSecret}")
            );
        }

        public static string PrepareOAuth1Request(string URL)
        {
            // seconds passed since 1/1/1970
            var timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

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

        static string GenerateSignature(string url, Dictionary<string, string> data)
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

        static string GenerateOAuthHeader(Dictionary<string, string> data)
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
