using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThuVienDienTu.Helper
{
    public class Configuration
    {
        public static readonly string clientId;
        public static readonly string clientSecret;
        static Configuration()
        {
            var config = GetConfig();
        }
        public static Dictionary<string,string> GetConfig()
        {
            return PayPalService.GetPayPalConfig();
        }
        public static string GetAccessToken()
        {
            var accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }
        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}
