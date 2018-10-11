using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictivePolicingApp.Code.SentimentAnalysis
{
    public class SentimentResults
    {
        private string language;
        private string language_short;
        private int tweet_id;
        private List<string> keyPhrases;
        private List<string> entities;
        private double senti_score;

        public string getLanguage()
        {
            return language;
        }

        public void setLanguage(string language)
        {
            this.language = language;
        }

        public string getLanguage_short()
        {
            return language_short;
        }

        public void setLanguage_short(string language_short)
        {
            this.language_short = language_short;
        }

        public int getTweet_id()
        {
            return tweet_id;
        }

        public void setTweet_id(int tweet_id)
        {
            this.tweet_id = tweet_id;
        }

        public List<string> getKeyPhrases()
        {
            return keyPhrases;
        }

        public void setKeyPhrases(List<string> keyPhrases)
        {
            this.keyPhrases = keyPhrases;
        }

        public List<string> getEntities()
        {
            return entities;
        }

        public void setEntities(List<string> entities)
        {
            this.entities = entities;
        }

        public double getSenti_score()
        {
            return senti_score;
        }

        public void setSenti_score(double senti_score)
        {
            this.senti_score = senti_score;
        }
    }
}