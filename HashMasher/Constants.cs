namespace HashMasher
{
    public class Constants
    {
        public const string ConsumerKey = "JGHdcmLdKhNWmY1HATSA";
        public const string ConsumerSecret = "uNULF7obRMfJ0OY79Y6zs9hQsCK6l4wRlaQ3Jhp0RMI";
        public const string HashMasherAccessToken = @"12048422-kUDK5o3ouWkrfUT3fPaSFGSuDDe0wTSFtPCsIkRcG";
        public const string HashMasherAccessTokenSecret = @"9BRUjfHmmH3VyCrlXgw7xlYAd3MY1LfExPJsMaCiO4";

        public static Twitterizer.OAuthTokens PrflockOAuthTokens
        {
            get
            {
                return new Twitterizer.OAuthTokens
                {
                    AccessToken = HashMasherAccessToken,
                    AccessTokenSecret = HashMasherAccessTokenSecret,
                    ConsumerKey = ConsumerKey,
                    ConsumerSecret = ConsumerSecret
                };
            }
        }
    }
}