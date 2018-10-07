using PredictivePolicingApp.Code.SentimentAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HonoursTestProject
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GuestGeek_DBService.Service service = new GuestGeek_DBService.Service();

            GuestGeek_DBService.CrimeTweets newTweet = service.getCrimeTweet(6, true);

            List<GuestGeek_DBService.CrimeTweets> newTweetList = new List<GuestGeek_DBService.CrimeTweets>();
            newTweetList.Add(newTweet);

            TextAnalytics newAnalysis = new TextAnalytics();
            newAnalysis.extractingLanguage(newTweetList);

            testPara.InnerHtml = newTweet.message;

        }
    }
}