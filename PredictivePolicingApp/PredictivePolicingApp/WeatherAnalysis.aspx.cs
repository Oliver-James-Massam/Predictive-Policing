using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PredictivePolicingApp
{
    public partial class WeatherAnalysis : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            refreshData();
        }

        private void refreshData()
        {
            weatherResults.InnerHtml += "<div class='card mb-4'>" +
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
                                                                "<th>ID</th>" +
                                                                "<th>Sentiment Total</th>" +
                                                                "<th>Primary Category</th>" +
                                                                "<th>Key Phrases</th>" +
                                                            "</tr>" +
                                                        "</thead>" +
                                                        "<tbody>" +
                                                            "<tr>" +
                                                                "<td> 0001 </td>" +
                                                                "<td> Product Name 1</td>" +
                                                                "<td>Customer 1</td>" +
                                                                "<td>Complete</td>" +
                                                            "</tr>" +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</div>" +
                                            "</div>" +
                                       "</div>" +
                                    "</div>";
        }

        protected void RefreshPage_Click(object sender, EventArgs e)
        {
            refreshData();
        }
    }
}