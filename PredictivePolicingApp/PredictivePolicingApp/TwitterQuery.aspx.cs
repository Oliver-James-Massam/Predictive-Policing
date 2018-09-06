using PredictivePolicingApp.Code.Sentiment;
using PredictivePolicingApp.Code.SentimentAnalysis;
using PredictivePolicingApp.Code.Twitter;
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
            
            
        }

        protected void UpdatePage_Click(object sender, EventArgs e)
        {
            String category = txtCategory.Value;
            String twitterHandle = txtTwitterHandle.Value;
            String keyword = txtKeyword.Value;

            if(category.Equals(""))
            {
                category = "Crime";
            }

            if(twitterHandle.Equals(""))
            {
                twitterHandle = "S.A Crime Watch";
            }

            if(keyword.Equals(""))
            {
                keyword = "Crime";
            }

            List<GuestGeek_DBService.CrimeTweets> tweetQuery = Query.Search_SearchTweet(keyword);
            GuestGeek_DBService.ServiceClient service = new GuestGeek_DBService.ServiceClient();

            foreach (GuestGeek_DBService.CrimeTweets tweet in tweetQuery)
            {
                queryFeed.InnerHtml += /*"<div class='col-lg-4 mb-4 bg-default'>" +*/
                                        "<div class='card'>" +
                                            "<div class='card-header'>Received At: " + DateTime.Now + "</div>" +
                                                "<div class='card-block'>" +
                                                    "<p>" + tweet.message + "</p>" +
                                                "</div>" +
                                        //"</div>" +
                                        "</div>";
            }
            //Sentiment.processTweetSentiments(tweetQuery);
            TextAnalytics newAnalysis = new TextAnalytics();
            newAnalysis.fullAnalysis(tweetQuery);
        }

        private void searchQuery()
        {
            
        }
    }
}