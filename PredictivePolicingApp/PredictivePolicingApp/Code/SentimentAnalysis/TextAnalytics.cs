using System;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Collections.Generic;
using Microsoft.Rest;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PredictivePolicingApp.Code.Misc;
using System.Text;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace PredictivePolicingApp.Code.SentimentAnalysis
{
    public class TextAnalytics
    {
        //------------------------------------------------------------------- Azure

        /// <summary>
        /// Container for subscription credentials. Make sure to enter your valid key.
        /// </summary>
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                string key = TextAnalyticsKeys.getTextAnalyticsKey1();
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        public struct SentimentResults
        {
            public string language;
            public string language_short;
            public int tweet_id;
            public List<string> keyPhrases;
            public double senti_score;
        }

        public List<SentimentResults> extractingLanguage(List<DB_Service.CrimeTweets> crimeTweets)
        {
            // Create a client.---------------------------------------------------------------------------------------------------------------------------------
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/"
            };

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Extracting language.---------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("===== LANGUAGE EXTRACTION ======");

            List<Input> myInp = new List<Input>();
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                Input inp = new Input(ct.tweet_id.ToString(), ct.message);
                myInp.Add(inp);
            }

            var result = client.DetectLanguageAsync(new BatchInput(myInp)).Result;

            // Printing language results.
            List<SentimentResults> tweetLangs = new List<SentimentResults>();
            foreach (var document in result.Documents)
            {
                SentimentResults tl = new SentimentResults();
                tl.language_short = document.DetectedLanguages[0].Iso6391Name;
                tl.tweet_id = Int32.Parse(document.Id);
                tl.language = document.DetectedLanguages[0].Name;
                tweetLangs.Add(tl);
            }

            return tweetLangs;
        }

        //public List<SentimentResults> extractingKeyPhrases(List<GuestGeek_DBService.CrimeTweets> crimeTweets)
        //{
        //    // Create a client.---------------------------------------------------------------------------------------------------------------------------------
        //    ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
        //    {
        //        Endpoint = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/"
        //    };

        //    System.Console.OutputEncoding = System.Text.Encoding.UTF8;

        //    // Getting key phrases.---------------------------------------------------------------------------------------------------------------------------------
        //    Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

        //    List<MultiLanguageInput> keyPhrases = new List<MultiLanguageInput>();
        //    foreach (GuestGeek_DBService.CrimeTweets ct in crimeTweets)
        //    {
        //        MultiLanguageInput inp = new MultiLanguageInput(;
        //        myInp.Add(inp);
        //    }

        //    KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(
        //                new List<MultiLanguageInput>()
        //                {
        //                  new MultiLanguageInput("ja", "1", "猫は幸せ"),
        //                  new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
        //                  new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
        //                  new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
        //                })).Result;

        //    // Printing key phrases.
        //    foreach (var document in result2.Documents)
        //    {
        //        Console.WriteLine("Document ID: {0} ", document.Id);

        //        Console.WriteLine("\t Key phrases:");

        //        foreach (string keyphrase in document.KeyPhrases)
        //        {
        //            Console.WriteLine("\t\t" + keyphrase);
        //        }
        //    }
        //}

        //static async void MakeRequest()
        //{
        //    var client = new HttpClient();
        //    var queryString = HttpUtility.ParseQueryString(string.Empty);

        //    // Request headers
        //    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", TwitterKeys.getTextAnalyticsKey());

        //    var uri = "https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/languages?" + queryString;

        //    HttpResponseMessage response;

        //    // Request body
        //    byte[] byteData = Encoding.UTF8.GetBytes("{body}");

        //    using (var content = new ByteArrayContent(byteData))
        //    {
        //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        response = await client.PostAsync(uri, content);
        //    }
        //}

        public List<SentimentResults> fullAnalysis(List<DB_Service.CrimeTweets> crimeTweets)
        {
            // Create a client.---------------------------------------------------------------------------------------------------------------------------------
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/"
            };

            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Extracting language.---------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("===== LANGUAGE EXTRACTION ======");

            List<Input> myInp = new List<Input>();//Languages
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                //Sentiment.Sentiment.sanitize(ct.message);
                Input inp = new Input(ct.tweet_id.ToString(), ct.message);
                myInp.Add(inp);
            }

            LanguageBatchResult result = null;
            try
            {
                result = client.DetectLanguageAsync(new BatchInput(myInp)).Result;
                //Task.Run(() => client.DetectLanguageAsync(new BatchInput(myInp)).Result).Wait();
            }
            catch (AggregateException aex)
            {
                string messages = "";

                foreach (Exception ex in aex.InnerExceptions)
                {
                    messages += ex.Message + "\r\n";
                }

                Debug.WriteLine(messages);
            }

            

            // Printing language results.
            List<SentimentResults> tweetLangs = new List<SentimentResults>();//Language
            foreach (var document in result.Documents)
            {
                SentimentResults tr = new SentimentResults();
                tr.language_short = document.DetectedLanguages[0].Iso6391Name;
                tr.tweet_id = Int32.Parse(document.Id);
                tr.language = document.DetectedLanguages[0].Name;
                tweetLangs.Add(tr);
            }

            // Getting key phrases.---------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

            List<MultiLanguageInput> keyPhrases = new List<MultiLanguageInput>();//Key phrases
            int count = 0;
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                string tempLang = tweetLangs.ElementAt<SentimentResults>(count).language_short;
                MultiLanguageInput inp = new MultiLanguageInput(tempLang, ct.tweet_id.ToString(), ct.message);
                keyPhrases.Add(inp);
                count++;
            }

            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(keyPhrases)).Result;

            // Printing key phrases.
            List<string> phrases = new List<string>();
            List<SentimentResults> tweetKeyPhrases = new List<SentimentResults>();
            count = 0;
            foreach (var document in result2.Documents)
            {
                //Console.WriteLine("Document ID: {0} ", document.Id);
                
                //Console.WriteLine("\t Key phrases:");

                foreach (string keyphrase in document.KeyPhrases)
                {
                    //Console.WriteLine("\t\t" + keyphrase);
                    phrases.Add(keyphrase);
                }
                SentimentResults sr = new SentimentResults();
                sr = tweetLangs.ElementAt<SentimentResults>(count);
                sr.keyPhrases = phrases;
                tweetKeyPhrases.Add(sr);
                count++;
            }

            // Analyzing sentiment.---------------------------------------------------------------------------------------------------------------------------------
            Console.WriteLine("\n\n===== SENTIMENT ANALYSIS ======");

            List<MultiLanguageInput> sentiAni = new List<MultiLanguageInput>();//Sentiment Analysis
            count = 0;
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                string tempLang = tweetKeyPhrases.ElementAt<SentimentResults>(count).language_short;
                MultiLanguageInput inp = new MultiLanguageInput(tempLang, ct.tweet_id.ToString(), ct.message);
                sentiAni.Add(inp);
                count++;
            }

            SentimentBatchResult result3 = client.SentimentAsync(new MultiLanguageBatchInput(sentiAni)).Result;

            // Printing sentiment results.
            List<SentimentResults> tweetSentiments = new List<SentimentResults>();
            count = 0;

            foreach (var document in result3.Documents)
            {
                //Console.WriteLine("Document ID: {0} , Sentiment Score: {1:0.00}", document.Id, document.Score);
                SentimentResults sr = new SentimentResults();
                sr = tweetKeyPhrases.ElementAt<SentimentResults>(count);
                sr.senti_score = (double)document.Score;
                tweetSentiments.Add(sr);
                count++;
            }

            List<DB_Service.Sentiments> completeSentiments = new List<DB_Service.Sentiments>();
            foreach(SentimentResults finalResults in tweetSentiments)
            {
                DB_Service.Sentiments newSenti = new DB_Service.Sentiments();
                newSenti.tweet_id = finalResults.tweet_id;
                newSenti.sentiment_total = finalResults.senti_score;
                newSenti.category_primary = finalResults.language + ", " + finalResults.language_short;

                StringBuilder wholePhrase = new StringBuilder("");
                count = 0;
                foreach(String word in finalResults.keyPhrases)
                {
                    count++;
                    if (finalResults.keyPhrases.Count > count)
                    {
                        wholePhrase.Append(word + ",");
                    }
                    else
                    {
                        wholePhrase.Append(word);
                    }
                }
                completeSentiments.Add(newSenti);
            }
            DB_Service.ServiceClient service = new DB_Service.ServiceClient();
            service.addSentiments(completeSentiments);
            return tweetSentiments;

            // Linking entities---------------------------------------------------------------------------------------------------------------------------------
            //Console.WriteLine("\n\n===== ENTITY LINKING ======");

            //EntitiesBatchResult result4 = client.EntitiesAsync(
            //        new MultiLanguageBatchInput(
            //            new List<MultiLanguageInput>()
            //            {
            //                new MultiLanguageInput("en", "0", "I really enjoy the new XBox One S. It has a clean look, it has 4K/HDR resolution and it is affordable."),
            //                new MultiLanguageInput("en", "1", "The Seattle Seahawks won the Super Bowl in 2014."),
            //            })).Result;

            //// Printing entity results.
            //foreach (var document in result4.Documents)
            //{
            //    Console.WriteLine("Document ID: {0} , Entities: {1}", document.Id, String.Join(", ", document.Entities.Select(entity => entity.Name)));
            //}
        }
    }
}