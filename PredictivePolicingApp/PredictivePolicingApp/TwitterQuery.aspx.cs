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

            List<DB_Service.CrimeTweets> tweetQuery = Query.Search_SearchTweet(keyword);
            DB_Service.ServiceClient service = new DB_Service.ServiceClient();

            foreach (DB_Service.CrimeTweets tweet in tweetQuery)
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
            //Sentiment.Test("I cant believe that Jane Killed someone in Johannesburg");
            //TextAnalyticsV2_1 newAnalysis = new TextAnalyticsV2_1();

            DB_Service.CrimeTweets newTweet = service.getCrimeTweet(6);
            List<DB_Service.CrimeTweets> newTweetList = new List<DB_Service.CrimeTweets>();
            newTweetList.Add(newTweet);

            List<SentimentResults> newResults = TextAnalyticsV2_1.analyseSentimentScore();
            // Printing language results.
            foreach (SentimentResults document in newResults)
            {
                testPara.InnerHtml += "Document ID: " + document.getTweet_id() + ", Score: " + document.getSenti_score() + "\n";
                //Console.WriteLine("Document ID: {0} , Language: {1}", document.Id, document.DetectedLanguages[0].Name);
            }
            //TextAnalyticsV2_1.fullAnalysis(newTweets);
        }

        private void searchQuery()
        {
            
        }
    }
}