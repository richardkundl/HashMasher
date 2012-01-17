using System.Linq;
using System.Web.Mvc;
using HashMasher.Model;
using ProMongoRepository;
using TwitterAuth;
using Twitterizer;
using log4net;

namespace HashMasher.Web.Controllers
{

    public class UsersController : TwitterController
    {
        private IMongoRepository<User> _userRepository;
        private static readonly ILog Logger = LogManager.GetLogger("UsersController");
    
        public override ActionResult Success()
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

        public override ActionResult Fail()
        {
            //throw new NotImplementedException();
            return View();
        }

        public override string ConsumerKey
        {
            get { return Constants.ConsumerKey; }
        }

        public override string ConsumerSecret
        {
            get { return Constants.ConsumerSecret; }
        }

    }
}
