using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TweetSharp;
using System.Timers;
using PredictivePolicingApp.Code.Misc;


namespace PredictivePolicingApp
{
    public partial class TwitterStream : System.Web.UI.Page
    {
        private static String customer_key = TwitterKeys.getCustomerKey();
        private static String customer_key_secret = TwitterKeys.getCustomerKeySecret();
        private static String access_token = TwitterKeys.getAccessToken();
        private static String access_token_secret = TwitterKeys.getAccessTokenSecret();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}