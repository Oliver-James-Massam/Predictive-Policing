using java.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VaderSharp;
using java.io;
using edu.stanford.nlp;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using static edu.stanford.nlp.ling.CoreAnnotations;
using Microsoft.Rest;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using PredictivePolicingApp.Code.Misc;
using System.Diagnostics;
using edu.stanford.nlp.simple;

namespace PredictivePolicingApp.Code.Sentiment
{
    public static class Sentiment
    {
        //clean the tweet
        public static string sanitize(string raw)
        {
            return Regex.Replace(raw, @"(@[A-Za-z0-9]+)|([^0-9A-Za-z \t])|(\w+:\/\/\S+)", " ").ToString();
        }
        //-------------------------------------------------------------------------- VaderSharp for Sentiment Score-------------------------------------
        //-- better for social media
        public static SentimentAnalysisResults runTweetAnalysis(string raw)
        {
           // var sanitized = sanitize(raw);

            SentimentIntensityAnalyzer analyzer = new SentimentIntensityAnalyzer();
            var results = analyzer.PolarityScores(raw);
            return results;
        }

        public static double getPositiveTweetAnalysis(string raw)
        {
            return runTweetAnalysis(raw).Positive;
        }

        public static double getNegativeTweetAnalysis(string raw)
        {
            return runTweetAnalysis(raw).Negative;
        }

        public static double getNeutralTweetAnalysis(string raw)
        {
            return runTweetAnalysis(raw).Neutral;
        }

        public static double getCompoundTweetAnalysis(string raw)
        {
            return runTweetAnalysis(raw).Compound;
        }

        //-------------------------------------------------------------------------------------- Stanford Core NLP -----------------------------------------
        //-- Better for Entity recognition

        public static void buildPipeline(string text)
        {//https://interviewbubble.com/getting-started-with-stanford-corenlp-a-stanford-corenlp-tutorial/

            // Path to the folder with models extracted from `stanford-corenlp-3.7.0-models.jar`
            var jarRoot = @"..\..\..\..\data\paket-files\nlp.stanford.edu\stanford-corenlp-full-2016-10-31\models";
            // creates a StanfordCoreNLP object, with POS tagging, lemmatization,
            // NER, parsing, and coreference resolution
            Properties props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            StanfordCoreNLP pipeline = new StanfordCoreNLP(props);

            // create an empty Annotation just with the given text
            Annotation document = new Annotation(text);

            // run all Annotators on this text
            pipeline.annotate(document);
            //Finished processing the document here
            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(document, new PrintWriter(stream));
                Debug.WriteLine(stream.toString());
                stream.close();
            }
        }

        public static List<DB_Service.Sentiments> processTweetSentiments(List<DB_Service.CrimeTweets> crimeTweets)
        {
            List<DB_Service.Sentiments> mySentiments = new List<DB_Service.Sentiments>();
            foreach(DB_Service.CrimeTweets ct in crimeTweets)
            {
                DB_Service.Sentiments senti = new DB_Service.Sentiments();
                senti.sentiment_total = getCompoundTweetAnalysis(ct.message);
                senti.category_primary = "To be Analysed";
                senti.key_phrases = "To be Analysed";
                senti.tweet_id = ct.tweet_id;
                mySentiments.Add(senti);
            }

            if(mySentiments != null)
            {
                DB_Service.ServiceClient service = new DB_Service.ServiceClient();

                service.addSentiments(mySentiments);
            }

            return mySentiments;
        }
        
        public static void Test(string tester)
        {
            var sanit = sanitize(tester);
            Sentence sentence = new Sentence(sanit);
            Debug.WriteLine(sentence.sentiment() + ""+ " ");
        }
    }
}