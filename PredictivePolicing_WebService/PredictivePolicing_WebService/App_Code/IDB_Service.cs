
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{

	[OperationContract]
	string GetData(int value);

    [OperationContract]
    string TestConnection();

    [OperationContract]
	CompositeType GetDataUsingDataContract(CompositeType composite);

    // TODO: Add your service operations here
    //---------------------------------------- Crime Tweets -----------------------------------------------------
    [OperationContract]
    int getTextAnalyticsStartPoint();

    [OperationContract]
    List<CrimeTweets> getCrimeTweetsToAnalyse();

    [OperationContract]
    List<CrimeTweets> getCrimeTweets();

    [OperationContract]
    CrimeTweets getCrimeTweet(int tweet_id);

    [OperationContract]
    int setCrimeTweet(CrimeTweets crime_tweet);

    [OperationContract]
    int addCrimeTweet(CrimeTweets crime_tweet);

    [OperationContract]
    int addCrimeTweets(List<CrimeTweets> crime_tweets);

    [OperationContract]
    int deleteCrimeTweet(int tweet_id);

    //---------------------------------------- SVM -----------------------------------------------------
    [OperationContract]
    List<SVM> getSVMs();

    [OperationContract]
    SVM getSVM(int sv_id);

    [OperationContract]
    int setSVM(SVM svm);

    [OperationContract]
    int addSVM(SVM svm);

    [OperationContract]
    int deleteSVM(int sv_id);

    //---------------------------------------- Sentiments -----------------------------------------------------
    [OperationContract]
    List<Sentiments> getSentiments();

    [OperationContract]
    Sentiments getSentiment(int sentiment_id);

    [OperationContract]
    int setSentiment(Sentiments sentiment);

    [OperationContract]
    int addSentiment(Sentiments sentiment);

    [OperationContract]
    int addSentiments(List<Sentiments> sentiments);

    [OperationContract]
    int deleteSentiment(int sentiment_id);

    //---------------------------------------- Entities -----------------------------------------------------
    [OperationContract]
    List<Entities> getEntities();

    [OperationContract]
    Entities getEntity(int entity_id);

    [OperationContract]
    int setEntity(Entities entity);

    [OperationContract]
    int addEntity(Entities entity);

    [OperationContract]
    int deleteEntity(int entity_id);

    //---------------------------------------- Other -----------------------------------------------------
    [OperationContract]
    string[] refreshDBData();
}

[DataContract]
public struct CrimeTweets
{
    [DataMember]
    public int tweet_id;
    [DataMember]
    public string message;
    [DataMember]
    public double latitude;
    [DataMember]
    public double longitude;
    [DataMember]
    public string location;
    [DataMember]
    public DateTime post_datetime;
    [DataMember]
    public DateTime recieved_datetime;
    [DataMember]
    public string twitter_handle;
    [DataMember]
    public string weather;
    [DataMember]
    public string mentions;
    [DataMember]
    public string tags;
}

[DataContract]
public struct SVM
{
    [DataMember]
    public int sv_id;
    [DataMember]
    public string support_vectors;
    [DataMember]
    public string alphas;
    [DataMember]
    public string weighted_sums;
    [DataMember]
    public string label;
    [DataMember]
    public string kernal;
    [DataMember]
    public int tweet_id;
}

[DataContract]
public struct Sentiments
{
    [DataMember]
    public int sentiment_id;
    [DataMember]
    public double sentiment_total;
    [DataMember]
    public string category_primary;
    [DataMember]
    public string key_phrases;
    [DataMember]
    public int tweet_id;
}

[DataContract]
public struct Entities
{
    [DataMember]
    public int entity_id;
    [DataMember]
    public string name;
    [DataMember]
    public string category_type;
    [DataMember]
    public double senti_score;
    [DataMember]
    public double senti_magnitude;
    [DataMember]
    public double senti_salience;
    [DataMember]
    public double sentiment_id;
}

// Use a data contract as illustrated in the sample below to add composite types to service operations.
[DataContract]
public class CompositeType
{
	bool boolValue = true;
	string stringValue = "Hello ";

    [DataMember]
	public bool BoolValue
	{
		get { return boolValue; }
		set { boolValue = value; }
	}

	[DataMember]
	public string StringValue
	{
		get { return stringValue; }
		set { stringValue = value; }
	}
}
