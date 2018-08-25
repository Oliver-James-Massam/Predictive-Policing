﻿using System;
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

    public int setCrimeTweet(CrimeTweets crime_tweet)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "UPDATE CrimeTweets SET message = @message, latitude = @latitude, longitude = @longitude, location = @location, post_datetime = @post_datetime, Attendence = @attendance, TableID = @tableID, Response = @response WHERE GuestID = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@message", SqlDbType.VarChar, 160).Value = crime_tweet.message;

        command.Parameters.Add("@latitude", SqlDbType.Decimal);
        command.Parameters["@latitude"].Precision = 38;
        command.Parameters["@latitude"].Scale = 19;
        command.Parameters["@latitude"].Value = crime_tweet.latitude;

        command.Parameters.Add("@longitude", SqlDbType.Decimal);
        command.Parameters["@longitude"].Precision = 38;
        command.Parameters["@longitude"].Scale = 19;
        command.Parameters["@longitude"].Value = crime_tweet.longitude;

        command.Parameters.Add("@location", SqlDbType.VarChar, 60).Value = crime_tweet.location;
        command.Parameters.Add("@post_datetime", SqlDbType.DateTime2, 27).Value = crime_tweet.post_datetime;
        command.Parameters.Add("@recieved_datetime", SqlDbType.DateTime2, 27).Value = crime_tweet.recieved_datetime;
        command.Parameters.Add("@twitter_handle", SqlDbType.VarChar, 20).Value = crime_tweet.twitter_handle;
        command.Parameters.Add("@weather", SqlDbType.VarChar, -1).Value = crime_tweet.weather;
        command.Parameters.Add("@mentions", SqlDbType.VarChar, -1).Value = crime_tweet.mentions;
        command.Parameters.Add("@tags", SqlDbType.VarChar, -1).Value = crime_tweet.tags;
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

    // <returns>ID of entry or negative for failure. -1 INSERT failure. -2 duplicate </returns>
    public int addCrimeTweet(CrimeTweets crime_tweet)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "INSERT INTO CrimeTweets (message, latitude, longitude, location, post_datetime, recieved_datetime, twitter_handle, weather, mentions, tags) " + //"OUTPUT INSERTED.tweet_id " +
                       "OUTPUT INSERTED.tweet_id " +
                       "VALUES (@message, @latitude, @longitude, @location, @post_datetime, @recieved_datetime, @twitter_handle, @weather, @mentions, @tags);";
        
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@message", SqlDbType.VarChar, 160).Value = crime_tweet.message;

        command.Parameters.Add("@latitude", SqlDbType.Decimal);
        command.Parameters["@latitude"].Precision = 38;
        command.Parameters["@latitude"].Scale = 19;
        command.Parameters["@latitude"].Value = crime_tweet.latitude;

        command.Parameters.Add("@longitude", SqlDbType.Decimal);
        command.Parameters["@longitude"].Precision = 38;
        command.Parameters["@longitude"].Scale = 19;
        command.Parameters["@longitude"].Value = crime_tweet.longitude;

        command.Parameters.Add("@location", SqlDbType.VarChar, 60).Value = crime_tweet.location;
        command.Parameters.Add("@post_datetime", SqlDbType.DateTime2, 27).Value = crime_tweet.post_datetime;
        command.Parameters.Add("@recieved_datetime", SqlDbType.DateTime2, 27).Value = crime_tweet.recieved_datetime;
        command.Parameters.Add("@twitter_handle", SqlDbType.VarChar, 20).Value = crime_tweet.twitter_handle;
        command.Parameters.Add("@weather", SqlDbType.VarChar, -1).Value = crime_tweet.weather;
        command.Parameters.Add("@mentions", SqlDbType.VarChar, -1).Value = crime_tweet.mentions;
        command.Parameters.Add("@tags", SqlDbType.VarChar, -1).Value = crime_tweet.tags;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            statusCode = (Int32)command.ExecuteScalar();
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

    public int deleteCrimeTweet(int tweet_id)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM CrimeTweets WHERE tweet_id = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = tweet_id;
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

    public List<SVM> getSVMs()
    {
        List<SVM> svms = new List<SVM>();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM SVM";
        SqlCommand command = new SqlCommand(query);
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Column Indexs
            int sv_idIndex = reader.GetOrdinal("sv_id");
            int support_vectorsIndex = reader.GetOrdinal("support_vectors");
            int alphasIndex = reader.GetOrdinal("alphas");
            int weighted_sumsIndex = reader.GetOrdinal("weighted_sums");
            int labelIndex = reader.GetOrdinal("label");
            int kernalIndex = reader.GetOrdinal("kernal");
            int tweet_idIndex = reader.GetOrdinal("tweet_id");

            while (reader.Read())
            {
                SVM svm = new SVM();
                svm.sv_id = reader.GetInt32(sv_idIndex);
                svm.support_vectors = reader.GetString(support_vectorsIndex);
                svm.alphas = reader.GetString(alphasIndex);
                svm.weighted_sums = reader.GetString(weighted_sumsIndex);
                svm.label = reader.GetString(labelIndex);
                svm.kernal = reader.GetString(kernalIndex);
                svm.tweet_id = reader.GetInt32(tweet_idIndex);
                svms.Add(svm);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return svms;
    }

    public SVM getSVM(int sv_id)
    {
        SVM mySVM = new SVM();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM SVM WHERE sv_id = @id";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = sv_id;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Coloumns Index
            int sv_idIndex = reader.GetOrdinal("sv_id");
            int support_vectorsIndex = reader.GetOrdinal("support_vectors");
            int alphasIndex = reader.GetOrdinal("alphas");
            int weighted_sumsIndex = reader.GetOrdinal("weighted_sums");
            int labelIndex = reader.GetOrdinal("label");
            int kernalIndex = reader.GetOrdinal("kernal");
            int tweet_idIndex = reader.GetOrdinal("tweet_id");

            if (reader.HasRows)
            {
                reader.Read();
                mySVM.sv_id = reader.GetInt32(sv_idIndex);
                mySVM.support_vectors = reader.GetString(support_vectorsIndex);
                mySVM.alphas = reader.GetString(alphasIndex);
                mySVM.weighted_sums = reader.GetString(weighted_sumsIndex);
                mySVM.label = reader.GetString(labelIndex);
                mySVM.kernal = reader.GetString(kernalIndex);
                mySVM.tweet_id = reader.GetInt32(tweet_idIndex);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return mySVM;
    }

    public int setSVM(SVM svm)
    {
        throw new NotImplementedException();
    }

    public int addSVM(SVM svm)
    {
        throw new NotImplementedException();
    }

    public int deleteSVM(int sv_id)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM SVM WHERE sv_id = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = sv_id;
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

    public List<Sentiments> getSentiments()
    {
        List<Sentiments> sentiments = new List<Sentiments>();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Sentiments";
        SqlCommand command = new SqlCommand(query);
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Column Indexs
            int sentiment_idIndex = reader.GetOrdinal("sentiment_id");
            int sentiment_totalIndex = reader.GetOrdinal("sentiment_total");
            int category_primaryIndex = reader.GetOrdinal("category_primary");
            int key_phrasesIndex = reader.GetOrdinal("key_phrases");
            int tweet_idIndex = reader.GetOrdinal("tweet_id");

            while (reader.Read())
            {
                Sentiments sentiment = new Sentiments();
                sentiment.sentiment_id = reader.GetInt32(sentiment_idIndex);
                sentiment.sentiment_total = reader.GetDouble(sentiment_totalIndex);
                sentiment.category_primary = reader.GetString(category_primaryIndex);
                sentiment.key_phrases = reader.GetString(key_phrasesIndex);
                sentiment.tweet_id = reader.GetInt32(tweet_idIndex);
                sentiments.Add(sentiment);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return sentiments;
    }

    public Sentiments getSentiment(int sentiment_id)
    {
        Sentiments mySentiment = new Sentiments();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Sentiments WHERE sentiment_id = @id";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = sentiment_id;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Coloumns Index
            int sentiment_idIndex = reader.GetOrdinal("sentiment_id");
            int sentiment_totalIndex = reader.GetOrdinal("sentiment_total");
            int category_primaryIndex = reader.GetOrdinal("category_primary");
            int key_phrasesIndex = reader.GetOrdinal("key_phrases");
            int tweet_idIndex = reader.GetOrdinal("tweet_id");

            if (reader.HasRows)
            {
                reader.Read();
                mySentiment.sentiment_id = reader.GetInt32(sentiment_idIndex);
                mySentiment.sentiment_total = reader.GetDouble(sentiment_totalIndex);
                mySentiment.category_primary = reader.GetString(category_primaryIndex);
                mySentiment.key_phrases = reader.GetString(key_phrasesIndex);
                mySentiment.tweet_id = reader.GetInt32(tweet_idIndex);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return mySentiment;
    }

    public int setSentiment(Sentiments sentiment)
    {
        throw new NotImplementedException();
    }

    public int addSentiment(Sentiments sentiment)
    {
        throw new NotImplementedException();
    }

    public int deleteSentiment(int sentiment_id)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM Sentiments WHERE sentiment_id = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = sentiment_id;
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

    public List<Entities> getEntities()
    {
        List<Entities> entities = new List<Entities>();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Entities";
        SqlCommand command = new SqlCommand(query);
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Column Indexs
            int entity_idIndex = reader.GetOrdinal("entity_id");
            int nameIndex = reader.GetOrdinal("name");
            int category_typeIndex = reader.GetOrdinal("category_type");
            int senti_scoreIndex = reader.GetOrdinal("senti_score");
            int senti_magnitudeIndex = reader.GetOrdinal("senti_magnitude");
            int senti_salienceIndex = reader.GetOrdinal("senti_salience");
            int sentiment_idIndex = reader.GetOrdinal("sentiment_id");

            while (reader.Read())
            {
                Entities entity = new Entities();
                entity.entity_id = reader.GetInt32(entity_idIndex);
                entity.name = reader.GetString(nameIndex);
                entity.category_type = reader.GetString(category_typeIndex);
                entity.senti_score = reader.GetDouble(senti_scoreIndex);
                entity.senti_magnitude = reader.GetDouble(senti_magnitudeIndex);
                entity.senti_salience = reader.GetDouble(senti_salienceIndex);
                entity.sentiment_id = reader.GetInt32(sentiment_idIndex);
                entities.Add(entity);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return entities;
    }

    public Entities getEntity(int entity_id)
    {
        Entities myEntity = new Entities();

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "SELECT * FROM Entities WHERE entity_id = @id";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = entity_id;
        command.Connection = connection;
        command.CommandType = CommandType.Text;

        try
        {
            command.Connection.Open();
            command.Prepare();
            SqlDataReader reader = command.ExecuteReader();
            //Coloumns Index
            int entity_idIndex = reader.GetOrdinal("entity_id");
            int nameIndex = reader.GetOrdinal("name");
            int category_typeIndex = reader.GetOrdinal("category_type");
            int senti_scoreIndex = reader.GetOrdinal("senti_score");
            int senti_magnitudeIndex = reader.GetOrdinal("senti_magnitude");
            int senti_salienceIndex = reader.GetOrdinal("senti_salience");
            int sentiment_idIndex = reader.GetOrdinal("sentiment_id");

            if (reader.HasRows)
            {
                reader.Read();
                myEntity.entity_id = reader.GetInt32(entity_idIndex);
                myEntity.name = reader.GetString(nameIndex);
                myEntity.category_type = reader.GetString(category_typeIndex);
                myEntity.senti_score = reader.GetDouble(senti_scoreIndex);
                myEntity.senti_magnitude = reader.GetDouble(senti_magnitudeIndex);
                myEntity.senti_salience = reader.GetDouble(senti_salienceIndex);
                myEntity.sentiment_id = reader.GetInt32(sentiment_idIndex);
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
        }
        command.Connection.Close();
        command.Dispose();
        connection.Dispose();
        return myEntity;
    }

    public int setEntity(Entities entity)
    {
        throw new NotImplementedException();
    }

    public int addEntity(Entities entity)
    {
        throw new NotImplementedException();
    }

    public int deleteEntity(int entity_id)
    {
        int statusCode = -1;

        SqlConnection connection = new SqlConnection(connectionString);
        string query = "DELETE FROM Entities WHERE entity_id = @id;";
        SqlCommand command = new SqlCommand(query);
        command.Parameters.Add("@id", SqlDbType.Int);
        command.Parameters["@id"].Value = entity_id;
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
