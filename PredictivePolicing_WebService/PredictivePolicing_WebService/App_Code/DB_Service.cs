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

    public CrimeTweets getCrimeTweet(int tweet_id)
    {
        CrimeTweets mytweet = new CrimeTweets();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM CrimeTweets WHERE tweet_id = @id";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = tweet_id;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Coloumns Index
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

            if (reader.HasRows)
            {
                reader.Read();
                mytweet.tweet_id = reader.GetInt32(reader.GetOrdinal("tweet_id"));
                mytweet.tweet_id = reader.GetInt32(tweetIDIndex);
                mytweet.message = reader.GetString(messageIndex);
                mytweet.latitude = getDoubleSafe(reader, latitudeIndex);//Nullable
                mytweet.longitude = getDoubleSafe(reader, longitudeIndex);//Nullable
                mytweet.location = getStringSafe(reader, locationIndex);// Nullable
                mytweet.post_datetime = reader.GetDateTime(postDatetimeIndex);
                mytweet.recieved_datetime = reader.GetDateTime(recievedDatetimeIndex);
                mytweet.twitter_handle = reader.GetString(twitterHandleIndex);
                mytweet.weather = getStringSafe(reader, weatherIndex);// Nullable
                mytweet.mentions = getStringSafe(reader, mentionsIndex);// Nullable
                mytweet.tags = getStringSafe(reader, tagsIndex);// Nullable
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return mytweet;
    }

    public string setCrimeTweet(CrimeTweets crime_tweet)
    {
        SqlConnection connection = new SqlConnection(connectionString);
        string query = "UPDATE CrimeTweets SET Title = @title, Name = @name, Surname = @surname, Email = @email, QRCode = @qrcode, Attendence = @attendance, TableID = @tableID, Response = @response WHERE GuestID = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters.Add("@title", SqlDbType.VarChar, 10);
        command.Parameters.Add("@name", SqlDbType.VarChar, 100);
        command.Parameters.Add("@surname", SqlDbType.VarChar, 100);
        command.Parameters.Add("@email", SqlDbType.VarChar, 255);
        command.Parameters.Add("@attendance", SqlDbType.Bit);
        command.Parameters.Add("@tableID", SqlDbType.Int);
        command.Parameters.Add("@response", SqlDbType.VarChar, 15);
        command.Parameters.Add("@qrcode", SqlDbType.VarChar, 255);
        command.Parameters["@id"].Value = guest.guestID;
        command.Parameters["@title"].Value = guest.title;
        command.Parameters["@name"].Value = guest.name;
        command.Parameters["@surname"].Value = guest.surname;
        command.Parameters["@email"].Value = guest.email;
        command.Parameters["@attendance"].Value = guest.attendance;
        command.Parameters["@tableID"].Value = guest.tableID;
        command.Parameters["@response"].Value = guest.response;
        command.Parameters["@qrcode"].Value = guest.qrCode;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            statusCode = command.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();

        return statusCode;
    }

    public string addCrimeTweet(CrimeTweets crime_tweet)
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

    public string setSVM(SVM svm)
    {
        throw new NotImplementedException();
    }

    public string addSVM(SVM svm)
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

    public string setSentiment(Sentiments sentiment)
    {
        throw new NotImplementedException();
    }

    public string addSentiment(Sentiments sentiment)
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

    public string setEntity(Entities entity)
    {
        throw new NotImplementedException();
    }

    public string addEntity(Entities entity)
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

    //Function was implemented when the class was created
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
}
