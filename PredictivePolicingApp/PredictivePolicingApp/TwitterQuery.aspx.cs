﻿using PredictivePolicingApp.Code.Twitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tweetinvi;

namespace PredictivePolicingApp
{
    public partial class TwitterQuery : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            List<String> tweetQuery = Query.Search_SearchTweet();
            
            foreach (String text in tweetQuery)
            {
                queryFeed.InnerHtml += /*"<div class='col-lg-4 mb-4 bg-default'>" +*/
                                        "<div class='card'>" +
                                            "<div class='card-header'>Recieved At: " + DateTime.Now + "</div>" +
                                                "<div class='card-block'>" +
                                                    "<p>" + text + "</p>" +
                                                "</div>" +
                                        //"</div>" +
                                        "</div>";
            }
            
        }

        //private void searchQuery()
        //{
        //    var tokens = new Twitterizer.OAuthTokens
        //    {
        //        ConsumerKey = @"consumerKey",
        //        ConsumerSecret = @"consumerSecret",
        //        AccessToken = @"accessToken",
        //        AccessTokenSecret = @"accessTokenSecret"
        //    };

        //    var response = Twitterizer.TwitterSearch.Search(tokens, "test",
        //      new Twitterizer.SearchOptions
        //      {
        //          GeoCode = "51.50788772102843,-0.102996826171875,50mi"
        //      });
        //    if (response.Result != Twitterizer.RequestResult.Success)
        //        return;

        //    foreach (var status in response.ResponseObject)
        //    {
        //        Console.WriteLine(status.Text);
        //    }
        //}
    }
}