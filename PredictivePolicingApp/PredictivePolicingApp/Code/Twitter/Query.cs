using PredictivePolicingApp.Code.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace PredictivePolicingApp.Code.Twitter
{
    public static class Query
    {
        private static Boolean hasSetUserCred = false;

        private static readonly double[][] JOHANNESBURG = new double[][] { new double[] { 27.726223, -26.391012 }, new double[] { 28.519985, -25.870720 } };//Longitude and Latitude
        private static readonly String[] TRACK = {"S.A Crime Watch", "@crimewatch202", "crimewatch202", 
											"@CrimeLineZA", "#FarmAttacks", "#FarmMurders", "South Africa", "#WhiteGenocide",
											"Johannesburg", "Joburg", "Jozi", "#StopFarmAttacks", "#StartFarmMurders", "#ShootTheBoer", "#WhiteMinorityGenocide",
											"#plaasmoorde"};

        private static void checkHasSetUserCred()
        {
            if(hasSetUserCred == false)
            {
                Auth.SetUserCredentials(TwitterKeys.getConsumerKey(), TwitterKeys.getConsumerKeySecret(), TwitterKeys.getAccessToken(), TwitterKeys.getAccessTokenSecret());
                hasSetUserCred = true;
            }
        }

        public static List<String> Search_SearchTweet()
        {
            checkHasSetUserCred();
            var searchParameter = Search.CreateTweetSearchParameter("crime");

            //searchParameter.SetGeoCode(new Coordinates(-26.195246, 28.034088), 100, DistanceMeasure.Kilometers);
            //searchParameter.Lang = LanguageFilter.English;
            //searchParameter.SearchType = SearchResultType.Popular;
            //searchParameter.MaximumNumberOfResults = 100;
            //searchParameter.Since = new DateTime(2013, 12, 1);
            //searchParameter.Until = new DateTime(2013, 12, 11);
            //searchParameter.SinceId = 399616835892781056;
            //searchParameter.MaxId = 405001488843284480;
            searchParameter.TweetSearchType = TweetSearchType.OriginalTweetsOnly;
            //searchParameter.Filters = TweetSearchFilters.Videos;

            var tweets = Search.SearchTweets(searchParameter);
            List<String> tweetText = new List<String>();
            if (tweets == null)
            {
                tweetText.Add(ExceptionHandler.GetLastException().TwitterDescription);
            }
            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    tweetText.Add(tweet.Text);
                }
            }
            return tweetText;
        }
    }
}