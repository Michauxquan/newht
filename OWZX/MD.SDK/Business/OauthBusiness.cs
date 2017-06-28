using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MD.SDK.Business
{
    public class OauthBusiness
    {
        public static string GetAuthorizeUrl()
        {
            string url = string.Empty;
            url = string.Format("{0}/oauth2/authorize?app_key={1}&redirect_uri={2}", AppAttr.MDApiUrl, AppAttr.AppKey, AppAttr.CallBackUrl);

            return url;
        }

        public static UserJson GetMDUser(string code)
        {
            var paras = new Dictionary<string, object>();
            paras.Add("code", code);
            paras.Add("redirect_uri", AppAttr.CallBackUrl);
            paras.Add("grant_type", "authorization_code");
            paras.Add("app_key", AppAttr.AppKey);
            paras.Add("app_secret", AppAttr.AppSecret);
            var result = HttpRequest.RequestServer(ApiOption.oauth2_access_token, paras, RequestType.Post);

            var tokenEntity = JsonConvert.DeserializeObject<TokenEntity>(result);
            string token = tokenEntity.access_token;
            var model = UserBusiness.GetPassportDetail(token);
            model.user.token = token;
            return model;
        }
    }
}
