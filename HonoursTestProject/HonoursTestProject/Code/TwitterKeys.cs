using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictivePolicingApp.Code.Misc
{
    public static class TwitterKeys
    {
        private readonly static String consumer_key = "weiJZegLHr7hIjIXFzhygO9CN";
        private readonly static String consumer_key_secret = "edAf82AagB9Fb9Nj24VqSYlSgM3iOWOxpiIwNODAxE22DHj3nO";
        private readonly static String access_token = "1499220372-x7CmJ9iTF9usYYWAsSlNbGKHgkTOKW3awxw558u";
        private readonly static String access_token_secret = "I7WdHCf3FcJPGVAI0mtyz62l0PGaKItcZXR0W9tp9vaKz";

        private readonly static String text_analytics_key = "34a2419468b64d5187367628ef50fdda";

        public static String getConsumerKey()
        {
            return consumer_key;
        }

        public static String getConsumerKeySecret()
        {
            return consumer_key_secret;
        }

        public static String getAccessToken()
        {
            return access_token;
        }

        public static String getAccessTokenSecret()
        {
            return access_token_secret;
        }

        public static String getTextAnalyticsKey()
        {
            return text_analytics_key;
        }
    }
}