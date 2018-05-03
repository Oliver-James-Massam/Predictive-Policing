import java.io.IOException;
import java.util.ArrayList;

import Twitter.TweetManager;
import twitter4j.FilterQuery;
import twitter4j.Query;
import twitter4j.StallWarning;
import twitter4j.Status;
import twitter4j.StatusDeletionNotice;
import twitter4j.StatusListener;
import twitter4j.TwitterException;
import twitter4j.TwitterStream;
import twitter4j.TwitterStreamFactory;

/**
 * 
 */

/**
 * @author Oliver-James Massam, 201472959
 *
 */
public class main {

	/**
	 * @param args
	 */
	public static void main(String[] args) 
	{
		//-------------Tweet Manager-----------------
		String topic = "crime";
		ArrayList<String> tweets = TweetManager.getTweets(topic);
		for(String tweet : tweets) 
		{
			System.out.println(tweet);
		}
	}
	
	/*//-------------Streaming Tweets-------------
	TwitterStream twitterStream = new TwitterStreamFactory().getInstance();
	StatusListener listener = new StatusListener(){
		@Override
        public void onStatus(Status status) {
            System.out.println("@" + status.getUser().getScreenName() + " - " + status.getText());
        }

        @Override
        public void onDeletionNotice(StatusDeletionNotice statusDeletionNotice) {
            System.out.println("Got a status deletion notice id:" + statusDeletionNotice.getStatusId());
        }

        @Override
        public void onTrackLimitationNotice(int numberOfLimitedStatuses) {
            System.out.println("Got track limitation notice:" + numberOfLimitedStatuses);
        }

        @Override
        public void onScrubGeo(long userId, long upToStatusId) {
            System.out.println("Got scrub_geo event userId:" + userId + " upToStatusId:" + upToStatusId);
        }

        @Override
        public void onStallWarning(StallWarning warning) {
            System.out.println("Got stall warning:" + warning);
        }

        @Override
        public void onException(Exception ex) {
            ex.printStackTrace();
        }
	};
	twitterStream.addListener(listener);
	//---------------------------------------------------------------------------------------
	//Filter
	FilterQuery tweetFilterQuery = new FilterQuery();
	//Twitter API works as an OR basis so it will track tweet keywords or location. Not keywords in a location
	//tweetFilterQuery.track(new String[]{"Crime", "Theft", "Murder",  "Hijacking", "Hit and Run", "Burglary", "Rape", "Assualt"});
	tweetFilterQuery.language("en"); // Note that language does not work properly on Norwegian tweets 
	
	double[][] longLat = new double[][]{new double[]{-25.870720,27.726223}, new double[]{-26.391012,28.519985}};
	tweetFilterQuery.locations(longLat);
	//Note that not all tweets have location metadata set.
	twitterStream.filter(tweetFilterQuery);
	//----------------------------------------------------------------------------------------
	//Start Streaming
	// sample() method internally creates a thread which manipulates TwitterStream and calls these adequate listener methods continuously.
	twitterStream.sample();*/
	
}
