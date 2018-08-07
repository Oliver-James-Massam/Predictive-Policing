using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TweetSharp;
using System.Timers;
using PredictivePolicingApp.Code.Misc;
using System.Net;

namespace PredictivePolicingApp
{
    public partial class TwitterStream : System.Web.UI.Page
    {
        private static String customer_key = TwitterKeys.getConsumerKey();
        private static String customer_key_secret = TwitterKeys.getConsumerKeySecret();
        private static String access_token = TwitterKeys.getAccessToken();
        private static String access_token_secret = TwitterKeys.getAccessTokenSecret();

        private static TwitterService service = new TwitterService(customer_key, customer_key_secret, access_token, access_token_secret);

        protected void Page_Load(object sender, EventArgs e)
        {
            streamFeed.InnerHtml += /*"<div class='col-lg-4 mb-4 bg-default'>" +*/
                                        "<div class='card'>" +
                                            "<div class='card-header'>Recieved At: " + DateTime.Now + "</div>" +
                                                "<div class='card-block'>" +
                                                    "<p></p>" +
                                                "</div>" +
                                            //"</div>" +
                                        "</div>";
        }

        private static void SendTweet(String status)
        {
            service.SendTweet(new SendTweetOptions { Status = status}, (tweet, response) =>
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //inner html to display success
                    //https://youtu.be/n2FadWBTL9E?t=8m3s
                }
                else
                {
                    //display error
                }
            });
        }

        private void getAllTweets()
        {
            //var currentTweets = service.str
        }
    }
}