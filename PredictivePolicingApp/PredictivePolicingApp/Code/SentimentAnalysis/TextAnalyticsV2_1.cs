using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;
using PredictivePolicingApp.Code.Misc;
using System.Text;

namespace PredictivePolicingApp.Code.SentimentAnalysis
{
    public static class TextAnalyticsV2_1
    {
        class ApiKeyServiceClientCredentials : ServiceClientCredentials
        {
            public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                string key = TextAnalyticsKeys.getTextAnalyticsKey1();
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                return base.ProcessHttpRequestAsync(request, cancellationToken);
            }
        }

        public static List<SentimentResults> fullAnalysis(List<DB_Service.CrimeTweets> crimeTweets)
        {//-------------------------------------------------------------------------------------------------------------------------------------
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com"
                //https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0
            }; 

            //-------------------------------------------------------------------------------------------------------------------------------------
            // Extracting language

            List<Input> myInp = new List<Input>();
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                Input inp = new Input(ct.tweet_id.ToString(), ct.message);
                myInp.Add(inp);
            }

            var result = client.DetectLanguageAsync(new BatchInput(myInp)).Result;

            List<SentimentResults> tweetLangs = new List<SentimentResults>();
            // Printing language results.
            foreach (var document in result.Documents)
            {
                SentimentResults sr = new SentimentResults();
                sr.setTweet_id(Int32.Parse(document.Id));
                sr.setLanguage_short(document.DetectedLanguages[0].Iso6391Name);
                sr.setLanguage(document.DetectedLanguages[0].Name);
                tweetLangs.Add(sr);
            }
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Getting key-phrases

            List<MultiLanguageInput> keyPhrases = new List<MultiLanguageInput>();//Key phrases
            int count = 0;
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                string tempLang = tweetLangs.ElementAt<SentimentResults>(count).getLanguage_short();
                MultiLanguageInput inp = new MultiLanguageInput(tempLang, ct.tweet_id.ToString(), ct.message);
                keyPhrases.Add(inp);
                count++;
            }

            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(keyPhrases)).Result;

            // Printing keyphrases
            List<string> phrases = new List<string>();
            List<SentimentResults> tweetKeyPhrases = new List<SentimentResults>();
            count = 0;
            foreach (var document in result2.Documents)
            {
                foreach (string keyphrase in document.KeyPhrases)
                {
                    phrases.Add(keyphrase);
                }
                SentimentResults sr = new SentimentResults();
                sr = tweetLangs.ElementAt<SentimentResults>(count);
                sr.setKeyPhrases(phrases);
                tweetKeyPhrases.Add(sr);
                count++;
            }
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Getting Sentiment Analysis

            List<MultiLanguageInput> sentiAni = new List<MultiLanguageInput>();//Sentiment Analysis
            count = 0;
            foreach (DB_Service.CrimeTweets ct in crimeTweets)
            {
                string tempLang = tweetKeyPhrases.ElementAt<SentimentResults>(count).getLanguage_short();
                MultiLanguageInput inp = new MultiLanguageInput(tempLang, ct.tweet_id.ToString(), ct.message);
                sentiAni.Add(inp);
                count++;
            }

            SentimentBatchResult result3 = client.SentimentAsync(new MultiLanguageBatchInput(sentiAni)).Result;

            // Printing sentiment results
            List<SentimentResults> tweetSentiments = new List<SentimentResults>();
            count = 0;

            foreach (var document in result3.Documents)
            {
                SentimentResults sr = new SentimentResults();
                sr = tweetKeyPhrases.ElementAt<SentimentResults>(count);
                sr.setSenti_score((double)document.Score);
                tweetSentiments.Add(sr);
                count++;
            }

            //-------------------------------------------------------------------------------------------------------------------------------------
            // Getting Entities

            //Continue using the same list so languages wont change
            EntitiesBatchResult result4 = client.EntitiesAsync(new MultiLanguageBatchInput(sentiAni)).Result;

            // Printing entities results
            List<string> entitiySet = new List<string>();
            List<SentimentResults> tweetEntities = new List<SentimentResults>();
            count = 0;

            foreach (var document in result4.Documents)
            {
                foreach (EntityRecord entitiy in document.Entities)
                {
                    entitiySet.Add(entitiy.Name);
                }
                SentimentResults sr = new SentimentResults();
                sr = tweetSentiments.ElementAt<SentimentResults>(count);
                sr.setEntities(entitiySet);
                tweetEntities.Add(sr);
                count++;
            }

            //-------------------------------------------------------------------------------------------------------------------------------------
            //Add Data to Database Service

            List<DB_Service.Sentiments> completeSentiments = new List<DB_Service.Sentiments>();
            List<DB_Service.Entities> completeEntities = new List<DB_Service.Entities>();
            foreach (SentimentResults finalResults in tweetEntities)
            {
                //Start building Sentiment class
                DB_Service.Sentiments newSenti = new DB_Service.Sentiments();
                newSenti.tweet_id = finalResults.getTweet_id();
                newSenti.sentiment_total = finalResults.getSenti_score();
                newSenti.category_primary = finalResults.getLanguage() + ", " + finalResults.getLanguage_short();

                List<string> entList = finalResults.getEntities();
                List<string> phraseList = finalResults.getKeyPhrases();
                StringBuilder wholePhrase = new StringBuilder("");
                count = 0;
                //Start building Entity Class
                if (entList != null && entList.Count > 0)
                {
                    foreach (string entity in entList)
                    {

                        wholePhrase.Append(entity + ",");
                        //DB_Service.Entities newEntity = new DB_Service.Entities();
                        //newEntity.name = entity;
                        //newEntity.sentiment_id = -1//this is a programming design problem
                    }
                }

                if (phraseList != null && phraseList.Count > 0)
                {
                    foreach (string word in phraseList)
                    {
                        count++;
                        if (phraseList.Count > count)
                        {
                            wholePhrase.Append(word + ",");
                        }
                        else
                        {
                            wholePhrase.Append(word);
                        }
                    }
                }
                    
                newSenti.key_phrases = wholePhrase.ToString();

                //List<string> EntList = finalResults.getEntities();
                //if(EntList != null && EntList.Count > 0)
                //{
                //    //newSenti.category_primary = EntList.ElementAt<string>(0);
                                       
                //}
                //else
                //{
                //    newSenti.category_primary = "";
                //}
                //Finish building Sentiment Class
                completeSentiments.Add(newSenti);

                
            }
            //Add to service now
            DB_Service.ServiceClient service = new DB_Service.ServiceClient();
            service.addSentiments(completeSentiments);

            return tweetEntities;
        }

        public static List<SentimentResults> analyseLanguageUsed()
        {//https://docs.microsoft.com/en-us/azure/cognitive-services/Text-Analytics/quickstarts/csharp
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com"
                //https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0
            }; //Replace 'westus' with the correct region for your Text Analytics subscription

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Extracting language
            Console.WriteLine("===== LANGUAGE EXTRACTION ======");

            var result = client.DetectLanguageAsync(new BatchInput(
            new List<Input>()
                {
                  new Input("1", "This is a document written in English."),
                  new Input("2", "Este es un document escrito en Español."),
                  new Input("3", "这是一个用中文写的文件")
            })).Result;

            List<SentimentResults> tweetLangs = new List<SentimentResults>();
            // Printing language results.
            foreach (var document in result.Documents)
            {
                SentimentResults sr = new SentimentResults();
                sr.setTweet_id(Int32.Parse(document.Id));
                sr.setLanguage_short(document.DetectedLanguages[0].Iso6391Name);
                sr.setLanguage(document.DetectedLanguages[0].Name);
                tweetLangs.Add(sr);
            }

            return tweetLangs;
        }

        public static List<string> analyseKeyPhrases()
        {//https://docs.microsoft.com/en-us/azure/cognitive-services/Text-Analytics/quickstarts/csharp
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com"
                //https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0
            }; //Replace 'westus' with the correct region for your Text Analytics subscription

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //-------------------------------------------------------------------------------------------------------------------------------------
            // Getting key-phrases
            //Console.WriteLine("\n\n===== KEY-PHRASE EXTRACTION ======");

            KeyPhraseBatchResult result2 = client.KeyPhrasesAsync(new MultiLanguageBatchInput(/*keyPhrases*/
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("ja", "1", "猫は幸せ"),
                          new MultiLanguageInput("de", "2", "Fahrt nach Stuttgart und dann zum Hotel zu Fu."),
                          new MultiLanguageInput("en", "3", "My cat is stiff as a rock."),
                          new MultiLanguageInput("es", "4", "A mi me encanta el fútbol!")
                        })).Result;

            // Printing keyphrases
            List<string> phrases = new List<string>();
            foreach (var document in result2.Documents)
            {
                //Console.WriteLine("Document ID: {0} ", document.Id);

                //Console.WriteLine("\t Key phrases:");

                foreach (string keyphrase in document.KeyPhrases)
                {
                    //Console.WriteLine("\t\t" + keyphrase);
                    phrases.Add(keyphrase);
                }
            }

            return phrases;
        }

        public static List<SentimentResults> analyseSentimentScore()
        {//https://docs.microsoft.com/en-us/azure/cognitive-services/Text-Analytics/quickstarts/csharp
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com"
                //https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0
            }; //Replace 'westus' with the correct region for your Text Analytics subscription

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Extracting sentiment
            Console.WriteLine("\n\n===== SENTIMENT ANALYSIS ======");

            SentimentBatchResult result3 = client.SentimentAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "0", "I had the best day of my life."),
                          new MultiLanguageInput("en", "1", "This was a waste of my time. The speaker put me to sleep."),
                          new MultiLanguageInput("es", "2", "No tengo dinero ni nada que dar..."),
                          new MultiLanguageInput("it", "3", "L'hotel veneziano era meraviglioso. È un bellissimo pezzo di architettura."),
                        })).Result;


            // Printing sentiment results
            List<SentimentResults> scores = new List<SentimentResults>();
            foreach (var document in result3.Documents)
            {
                //Console.WriteLine("Document ID: {0} , Sentiment Score: {1:0.00}", document.Id, document.Score);
                SentimentResults results = new SentimentResults();
                results.setTweet_id(Int32.Parse(document.Id));
                results.setSenti_score(document.Score.HasValue ? document.Score.Value : 0);
                scores.Add(results);
            }
            return scores;
        }

        public static List<string> analyseEntities()
        {//https://docs.microsoft.com/en-us/azure/cognitive-services/Text-Analytics/quickstarts/csharp
            // Create a client.
            ITextAnalyticsClient client = new TextAnalyticsClient(new ApiKeyServiceClientCredentials())
            {
                Endpoint = "https://westeurope.api.cognitive.microsoft.com"
                //https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0
            }; //Replace 'westus' with the correct region for your Text Analytics subscription

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Identify entities
            Console.WriteLine("\n\n===== ENTITIES ======");

            EntitiesBatchResult result4 = client.EntitiesAsync(
                    new MultiLanguageBatchInput(
                        new List<MultiLanguageInput>()
                        {
                          new MultiLanguageInput("en", "0", "The Great Depression began in 1929. By 1933, the GDP in America fell by 25%.")
                        })).Result;

            // Printing entities results
            List<string> entities = new List<string>();
            foreach (var document in result4.Documents)
            {
                Console.WriteLine("Document ID: {0} ", document.Id);

                Console.WriteLine("\t Entities:");

                foreach (EntityRecord entity in document.Entities)
                {
                    Console.WriteLine("\t\t" + entity.Name);
                    entities.Add(entity.Name);
                }
            }
            return entities;
        }
    }
}