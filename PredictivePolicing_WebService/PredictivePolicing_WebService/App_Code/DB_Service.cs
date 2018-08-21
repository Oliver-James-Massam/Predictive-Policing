using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    //Connection String to the database
    private static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFileName=|DataDirectory|\PredictivePolicing_Database.mdf;Integrated Security=true;MultipleActiveResultSets=True";
    //default value for null
    private static readonly double NULL_DOUBLE = -1337;

    //Test if the connection to the web service exists
    public string GetData(int value)
	{
		return string.Format("You entered: {0}", value);
	}

    //Test if the connection to the Database is valid
    public string TestConnection()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return "Connection open";
        }
    }

    public CompositeType GetDataUsingDataContract(CompositeType composite)
	{
		if (composite == null)
		{
			throw new ArgumentNullException("composite");
		}
		if (composite.BoolValue)
		{
			composite.StringValue += "Suffix";
		}
		return composite;
	}

    //Get all tweets in the database
    public List<CrimeTweets> getCrimeTweets()
    {
        List<CrimeTweets> tweets = new List<CrimeTweets>();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM CrimeTweets";
        SqlCommand command = new SqlCommand(query);
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Column Indexs
            int tweetIDIndex = reader.GetOrdinal("tweet_id");
            int messageIndex = reader.GetOrdinal("message");
            int latitudeIndex = reader.GetOrdinal("latitude");
            int longitudeIndex = reader.GetOrdinal("longitude");
            int locationIndex = reader.GetOrdinal("location");
            int postDatetimeIndex = reader.GetOrdinal("post_datetime");
            int recievedDatetimeIndex = reader.GetOrdinal("recieved_datetime");
            int twitterHandleIndex = reader.GetOrdinal("twitter_handle");
            int weatherIndex = reader.GetOrdinal("weather");
            int mentionsIndex = reader.GetOrdinal("mentions");
            int tagsIndex = reader.GetOrdinal("tags");

            while(reader.Read())
            {
                CrimeTweets tweet = new CrimeTweets();
                tweet.tweet_id = reader.GetInt32(tweetIDIndex);
                tweet.message = reader.GetString(messageIndex);
                tweet.latitude = getDoubleSafe(reader, latitudeIndex);//Nullable
                tweet.longitude = getDoubleSafe(reader, longitudeIndex);//Nullable
                tweet.location = getStringSafe(reader, locationIndex);// Nullable
                tweet.post_datetime = reader.GetDateTime(postDatetimeIndex);
                tweet.recieved_datetime = reader.GetDateTime(recievedDatetimeIndex);
                tweet.twitter_handle = reader.GetString(twitterHandleIndex);
                tweet.weather = getStringSafe(reader, weatherIndex);// Nullable
                tweet.mentions = getStringSafe(reader, mentionsIndex);// Nullable
                tweet.tags = getStringSafe(reader, tagsIndex);// Nullable
                tweets.Add(tweet);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return tweets;
    }

    //Get String when the field can have a null value
    private string getStringSafe(SqlDataReader reader, int index)
    {
        if (!reader.IsDBNull(index))
            return reader.GetString(index);
        return string.Empty;
    }

    //Get Double when the field can have a null value
    private double getDoubleSafe(SqlDataReader reader, int index)
    {
        if (!reader.IsDBNull(index))
            return reader.GetDouble(index);
        return NULL_DOUBLE;
    }

    public CrimeTweets getCrimeTweet(int tweet_id)
    {
        throw new NotImplementedException();
    }

    public string setCrimeTweet(int tweet_id, string message, double latitude, double longitude, string location, DateTime post_datetime, DateTime recieved_datetime, string twitter_handle, string weather, string mentions, string tags)
    {
        throw new NotImplementedException();
    }

    public string addCrimeTweet(string message, double latitude, double longitude, string location, DateTime post_datetime, DateTime recieved_datetime, string twitter_handle, string weather, string mentions, string tags)
    {
        throw new NotImplementedException();
    }

    public string deleteCrimeTweet(int tweet_id)
    {
        throw new NotImplementedException();
    }

    public SVM[] getSVMs()
    {
        throw new NotImplementedException();
    }

    public SVM getSVM(int sv_id)
    {
        throw new NotImplementedException();
    }

    public string setSVM(int sv_id, string support_vectors, string alphas, string weighted_sums, string label, string kernal, int tweet_id)
    {
        throw new NotImplementedException();
    }

    public string addSVM(string support_vectors, string alphas, string weighted_sums, string label, string kernal, int tweet_id)
    {
        throw new NotImplementedException();
    }

    public string deleteSVM(int sv_id)
    {
        throw new NotImplementedException();
    }

    public Sentiments[] getSentiments()
    {
        throw new NotImplementedException();
    }

    public Sentiments getSentiment(int sentiment_id)
    {
        throw new NotImplementedException();
    }

    public string setSentiment(int sentiment_id, double sentiment_total, string category_primary, string key_phrases, int tweet_id)
    {
        throw new NotImplementedException();
    }

    public string addSentiment(double sentiment_total, string category_primary, string key_phrases, int tweet_id)
    {
        throw new NotImplementedException();
    }

    public string deleteSentiment(int sentiment_id)
    {
        throw new NotImplementedException();
    }

    public Entities[] getEntities()
    {
        throw new NotImplementedException();
    }

    public Entities getEntity(int entity_id)
    {
        throw new NotImplementedException();
    }

    public string setEntity(int entity_id, string name, string category_type, double senti_score, double senti_magnitude, double senti_salience, int sentiment_id)
    {
        throw new NotImplementedException();
    }

    public string addEntity(string name, string category_type, double senti_score, double senti_magnitude, double senti_salience, int sentiment_id)
    {
        throw new NotImplementedException();
    }

    public string deleteEntity(int entity_id)
    {
        throw new NotImplementedException();
    }

    public string[] refreshDBData()
    {
        throw new NotImplementedException();
    }
}
