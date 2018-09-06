using PredictivePolicingApp.Code.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;
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

        public static List<GuestGeek_DBService.CrimeTweets> Search_SearchTweet(String keyword)
        {
            checkHasSetUserCred();
            var searchParameter = Search.CreateTweetSearchParameter(keyword);

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

            List<GuestGeek_DBService.CrimeTweets> crimeTweets = new List<GuestGeek_DBService.CrimeTweets>();

            if (tweets == null)
            {
                tweetText.Add(ExceptionHandler.GetLastException().TwitterDescription);
            }
            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    tweetText.Add(tweet.Text);
                    GuestGeek_DBService.CrimeTweets myTweet = new GuestGeek_DBService.CrimeTweets();
                    myTweet.message = tweet.Text;

                    ICoordinates newCood = tweet.Coordinates;
                    if(newCood == null)
                    {
                        myTweet.latitude = -1;
                        myTweet.longitude = -1;
                    }
                    else
                    {
                        myTweet.latitude = newCood.Latitude;
                        myTweet.longitude = newCood.Longitude;
                    }

                    IPlace newPlace = tweet.Place;
                    if(newPlace == null)
                    {
                        myTweet.location = "Not provided";
                    }
                    else
                    {
                        myTweet.location = tweet.Place.Name;
                    }


                    myTweet.post_datetime = tweet.CreatedAt;
                    myTweet.recieved_datetime = DateTime.Now;
                    myTweet.twitter_handle = tweet.Prefix;
                    myTweet.weather = "Needs to be Analysed";
                    List<IUserMentionEntity> myMention = tweet.UserMentions;
                    myTweet.mentions = "";
                    foreach (IUserMentionEntity ent in myMention)
                    {
                        if (myTweet.mentions.Length != 0)
                        {
                            myTweet.mentions += " ";
                        }
                        myTweet.mentions += ent.ScreenName;
                    }
                    myTweet.tags = "Needs to be analysed";
                    crimeTweets.Add(myTweet);
                }
                
            }
            GuestGeek_DBService.ServiceClient service = new GuestGeek_DBService.ServiceClient();

            service.addCrimeTweets(crimeTweets);
            return crimeTweets;
        }

        public static List<GuestGeek_DBService.CrimeTweets> Search_Tweet(String keyword)
        {
            checkHasSetUserCred();
            var searchParameter = Search.CreateTweetSearchParameter(keyword);

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

            List<GuestGeek_DBService.CrimeTweets> crimeTweets = new List<GuestGeek_DBService.CrimeTweets>();

            if (tweets == null)
            {
                tweetText.Add(ExceptionHandler.GetLastException().TwitterDescription);
            }
            if (tweets != null)
            {
                foreach (var tweet in tweets)
                {
                    tweetText.Add(tweet.Text);
                    GuestGeek_DBService.CrimeTweets myTweet = new GuestGeek_DBService.CrimeTweets();
                    myTweet.message = tweet.Text;

                    ICoordinates newCood = tweet.Coordinates;
                    if (newCood == null)
                    {
                        myTweet.latitude = -1;
                        myTweet.longitude = -1;
                    }
                    else
                    {
                        myTweet.latitude = newCood.Latitude;
                        myTweet.longitude = newCood.Longitude;
                    }

                    IPlace newPlace = tweet.Place;
                    if (newPlace == null)
                    {
                        myTweet.location = "Not provided";
                    }
                    else
                    {
                        myTweet.location = tweet.Place.Name;
                    }


                    myTweet.post_datetime = tweet.CreatedAt;
                    myTweet.recieved_datetime = DateTime.Now;
                    myTweet.twitter_handle = tweet.Prefix;
                    myTweet.weather = "Needs to be Analysed";
                    List<IUserMentionEntity> myMention = tweet.UserMentions;
                    myTweet.mentions = "";
                    foreach (IUserMentionEntity ent in myMention)
                    {
                        if (myTweet.mentions.Length != 0)
                        {
                            myTweet.mentions += " ";
                        }
                        myTweet.mentions += ent.ScreenName;
                    }
                    myTweet.tags = "Needs to be analysed";

                    crimeTweets.Add(myTweet);
                }

            }
            GuestGeek_DBService.ServiceClient service = new GuestGeek_DBService.ServiceClient();

            service.addCrimeTweets(crimeTweets);

            return crimeTweets;
        }
    }
}