using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HashMasher.Model;
using HashMasher.Web.Models;
using ProMongoRepository;
using Twitterizer;
using log4net;

namespace HashMasher.Web.Controllers
{

    public class UsersController : Controller
    {
        private IMongoRepository<User> _userRepository;
        private static readonly ILog Logger = LogManager.GetLogger("UsersController");
    
        public ActionResult Success()
        {
            Logger.Debug("Success Called");

            var twitterResponse = (ITwitterResponse)TempData["twitterResponse"];
            if (twitterResponse != null)
            {
                _userRepository = Container.Windsor.Resolve<IMongoRepository<User>>();
                var existing = _userRepository.Linq().FirstOrDefault(x => x.AccessSecret == twitterResponse.AccessSecret);
                if(existing==null)
                {
                    //add user to database.
                    var oauthTokens = new OAuthTokens
                    {
                        AccessToken = twitterResponse.AccessToken,
                        AccessTokenSecret = twitterResponse.AccessSecret,
                        ConsumerKey = ConsumerKey,
                        ConsumerSecret = ConsumerSecret
                    };
                    TwitterResponse<TwitterUser> response = TwitterUser.Show(oauthTokens, twitterResponse.Logon);
                     if (response.Result == RequestResult.Success)
                     {
                         var details = response.ResponseObject;
                         var user = new User()
                         {
                             TwitterId = details.Id.ToString(),
                             Logon = details.ScreenName,
                             Name = details.Name,
                             Location = details.Location,
                             AccessSecret = twitterResponse.AccessSecret,
                             AccessToken = twitterResponse.AccessToken,
                             IsActive = true,
                             ProfileImage = details.ProfileImageLocation
                         };
                         _userRepository.Save(user);
                     }
                    

                }
                else
                {
                    //user exists. yay.
                }
            }
            return Redirect("~/");
        }

        public  ActionResult Fail()
        {
            //throw new NotImplementedException();
            return View();
        }

        public  string ConsumerKey
        {
            get { return Constants.ConsumerKey; }
        }

        public  string ConsumerSecret
        {
            get { return Constants.ConsumerSecret; }
        }


        public void Logon()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            var callBackUrl = new Uri(request.Url.Scheme + "://" + request.Url.Authority + "/Users/Callback");
            OAuthTokenResponse oAuthTokenResponse = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, callBackUrl.ToString());
            var uri = OAuthUtility.BuildAuthorizationUri(oAuthTokenResponse.Token, true);
            Response.Redirect(uri.ToString());
        }

        public ActionResult Logoff()
        {
            var formsAuthenticationTicket = new FormsAuthenticationTicket(1, "", DateTime.Now, DateTime.Now.AddMinutes(-30), false, "", FormsAuthentication.FormsCookiePath);
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(formsAuthenticationTicket)));
            return Redirect("~/");
        }

        public ActionResult Callback()
        {
            OAuthTokenResponse response = null;

            if (Request.QueryString.AllKeys.ToList().Contains("oauth_token"))
            {
                //string oauthToken = Request.QueryString["oauth_token"].ToString();
                string requestToken = Request.QueryString["oauth_token"];
                string verifier = Request.QueryString["oauth_verifier"];
                response = OAuthUtility.GetAccessToken(ConsumerKey, ConsumerSecret, requestToken, verifier);
                //response = OAuthUtility.GetAccessTokenDuringCallback(ConsumerKey, ConsumerSecret);
            }



            if (response != null)
            {
                var twitterResponse = new TwitterResponse
                {
                    AccessSecret = response.TokenSecret,
                    AccessToken = response.Token,
                    Logon = response.ScreenName, //use consistent verbiage between username/logon/screen name.
                };

                CreateAuthCookie(response.ScreenName, response.Token);

                TempData["twitterResponse"] = twitterResponse;
                return RedirectToAction("Success");
            }
            return RedirectToAction("Fail", response);
        }

        private static void CreateAuthCookie(string username, string token)
        {
            //Get ASP.NET to create a forms authentication cookie (based on settings in web.config)~
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(username, false);

            //Decrypt the cookie
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

            //Create a new ticket using the details from the generated cookie, but store the username & token passed in from the authentication method
            FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration, ticket.IsPersistent, token);

            // Encrypt the ticket & store in the cookie
            cookie.Value = FormsAuthentication.Encrypt(newticket);

            // Update the outgoing cookies collection.
            System.Web.HttpContext.Current.Response.Cookies.Set(cookie);
        }
    }
}
