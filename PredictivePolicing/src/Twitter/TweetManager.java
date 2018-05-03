/**
 * 
 */
package Twitter;

import java.util.ArrayList;
import java.util.List;

import twitter4j.Paging;
import twitter4j.Query;
import twitter4j.QueryResult;
import twitter4j.Status;
import twitter4j.Twitter;
import twitter4j.TwitterException;
import twitter4j.TwitterFactory;

/**
 * @author Oliver-James Massam, 201472959
 *
 */
public class TweetManager 
{
	public static ArrayList<String> getTweetsPage() 
	{
		ArrayList<String> tweetList = new ArrayList<String>();
		
		Twitter twitter = new TwitterFactory().getInstance(); 
	    Paging paging = new Paging(1,40);
	    try{
	        List<Status> statuses = twitter.getUserTimeline("ewn", paging);

	        System.out.println(paging);
	        for(Status status : statuses)
	        {
	        	tweetList.add(status.getText());
	        }

//	        System.out.println("\n\n\n");
//	        paging.setPage(2);
//	        statuses = twitter.getUserTimeline("google",paging);
//
//	        for(Status status : statuses)
//	        {
//	        	tweetList.add(status.getText());
//	        }
	    }
	    catch(TwitterException e){
	        e.printStackTrace();
	    }
		return tweetList;
	}
	
	public static ArrayList<String> getTweets(String topic) 
	{
		Twitter twitter = new TwitterFactory().getInstance();
		ArrayList<String> tweetList = new ArrayList<String>();
		try 
		{
			Query query = new Query(topic);
			query.setLang("en");//English (ISO 639-1 code)
			QueryResult result;
			do 
			{
				result = twitter.search(query);
				List<Status> tweets = result.getTweets();
				for (Status tweet : tweets) 
				{
					tweetList.add(tweet.getText());
				}
			} while ((query = result.nextQuery()) != null);
		} 
		catch (TwitterException te) 
		{
			te.printStackTrace();
			System.out.println("Failed to search tweets: " + te.getMessage());
		}
		return tweetList;
	}
}
