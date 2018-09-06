using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Newtonsoft.Json;
using Tweetinvi;
using Tweetinvi.Models;
using PredictivePolicingApp.Code.Misc;
using Tweetinvi.Events;

namespace PredictivePolicingApp.Code.Twitter
{
    public static class TweetStream
    {
        private static Boolean hasSetUserCred = false;
        private static readonly double[][] JOHANNESBURG = new double[][] { new double[] { 27.726223, -26.391012 }, new double[] { 28.519985, -25.870720 } };//Longitude and Latitude
        private static readonly String[] TRACK = {"S.A Crime Watch", "crimewatch202", "crimewatch202", "crime", "murder", "rape", "stabbing", "stabbed", "hijacking",
                                            "CrimeLineZA", "FarmAttacks", "FarmMurders", "South Africa", "WhiteGenocide", "hijacked", "hijack", "stab", "mugged", "theft", "thief",
                                            "Johannesburg", "Joburg", "Jozi", "topFarmAttacks", "StartFarmMurders", "ShootTheBoer", "WhiteMinorityGenocide", "robbery", "robbed",
                                            "plaasmoorde", "killed", "kill", "killing", "assault", "assaulted", "beaten", "beat", "shot"};

        private static void checkHasSetUserCred()
        {
            if (hasSetUserCred == false)
            {
                Auth.SetUserCredentials(TwitterKeys.getConsumerKey(), TwitterKeys.getConsumerKeySecret(), TwitterKeys.getAccessToken(), TwitterKeys.getAccessTokenSecret());
                hasSetUserCred = true;
            }
        }

        //default stream
        private static void buildStream()
        {
            var stream = Stream.CreateFilteredStream();
            foreach(String t in TRACK)
            {
                stream.AddTrack(t);
            }
            stream.AddTweetLanguageFilter("en");
            
        }
        //customised
        private static void buildStream(List<String> tracked_keywords)
        {
            var stream = Stream.CreateFilteredStream();
            foreach (String t in tracked_keywords)
            {
                stream.AddTrack(t);
            }
            stream.AddTweetLanguageFilter("en");
        }

        private static void OnMatchedTweet(object sender, MatchedTweetReceivedEventArgs args)
        {
            Sentiment.Sentiment.runTweetAnalysis(args.Tweet.FullText);
        }

    }
}