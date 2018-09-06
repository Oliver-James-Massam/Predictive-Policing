using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PredictivePolicingApp
{
    public partial class SentimentAnalysis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            refreshData();
        }

        private void refreshData()
        {
            sentiResults.InnerHtml = "<div class='card mb-4'>" +
                                            "<div class='card-block'>" +
                                                "<h3 class='card-title'>Results</h3>" +
                                                "<div class='dropdown card-title-btn-container'>" +
                                                   "<div class='dropdown-menu dropdown-menu-right' aria-labelledby='dropdownMenuButton'> " +
                                                        "<a class='dropdown-item' href='#'><em class='fa fa-search mr-1'></em>More info</a>" +
                                                        "<a class='dropdown-item' href='#'><em class='fa fa-thumb-tack mr-1'></em>Pin Window</a>" +
                                                        "<a class='dropdown-item' href='#'><em class='fa fa-remove mr-1'></em>Close Window</a>" +
                                                    "</div>" +
                                                "</div>" +
                                                "<div class='table-responsive'>" +
                                                    "<table class='table table-striped'>" +
                                                        "<thead>" +
                                                            "<tr>" +
                                                                "<th>Sentiment ID</th>" +
                                                                "<th>Sentiment Total</th>" +
                                                                "<th>Primary Category</th>" +
                                                                "<th>Key Phrases</th>" +
                                                                "<th>Tweet ID</th>" +
                                                            "</tr>" +
                                                        "</thead>" +
                                                        "<tbody>";

            List<GuestGeek_DBService.Sentiments> sentiments = new List<GuestGeek_DBService.Sentiments>();
            GuestGeek_DBService.ServiceClient service = new GuestGeek_DBService.ServiceClient();

            sentiments = service.getSentiments();

            if(sentiments != null)
            {
                foreach (GuestGeek_DBService.Sentiments senti in sentiments)
                {
                    sentiResults.InnerHtml += "<tr>" +
                                                    "<td>" + senti.sentiment_id + "</td>" +
                                                    "<td>" + senti.sentiment_total + "/td>" +
                                                    "<td>" + senti.category_primary + "</td>" +
                                                    "<td>" + senti.key_phrases + "</td>" +
                                                    "<td>" + senti.tweet_id + "</td>" +
                                                "</tr>";
                }
            }           

                                sentiResults.InnerHtml += "</tbody>" +
                                                    "</table>"+
                                                "</div>"+
                                            "</div>"+
                                       "</div>"+
                                    "</div>";
        }

        protected void RefreshPage_Click(object sender, EventArgs e)
        {
            refreshData();
        }
    }
}