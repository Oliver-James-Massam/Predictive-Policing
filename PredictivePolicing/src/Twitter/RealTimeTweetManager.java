package Twitter;

import twitter4j.FilterQuery;
import twitter4j.StallWarning;
import twitter4j.Status;
import twitter4j.StatusDeletionNotice;
import twitter4j.StatusListener;
import twitter4j.TwitterStream;
import twitter4j.TwitterStreamFactory;

public class RealTimeTweetManager 
{
	private static final double[][] JOHANNESBURG = new double[][]{new double[]{27.726223,-26.391012}, new double[]{28.519985,-25.870720}};//Longitude and Latitude
	private static final String[] TRACK = {"S.A Crime Watch", "@crimewatch202", "crimewatch202", 
											"@CrimeLineZA", "#FarmAttacks", "#FarmMurders", "South Africa", "#WhiteGenocide",
											"Johannesburg", "Joburg", "Jozi", "#StopFarmAttacks", "#StartFarmMurders", "#ShootTheBoer", "#WhiteMinorityGenocide",
											"#plaasmoorde"};
	
	private static final String[] TRACK2 = {"Abuse", "Accomplice", "Aggravated assault", "Armed", "Arrest"};
	private static final String[] TRACK3 = {"Abuse", "Accomplice", "Aggravated assault", "Armed", "Arrest"};
	public static void main(String[] args) 
	{
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
		//tweetFilterQuery.language("en"); // Note that language does not work properly on Norwegian tweets 
		
		tweetFilterQuery.locations(JOHANNESBURG);
		//tweetFilterQuery.track(TRACK);
		//Note that not all tweets have location metadata set.
		twitterStream.filter(tweetFilterQuery);
		//----------------------------------------------------------------------------------------
		//Start Streaming
		// sample() method internally creates a thread which manipulates TwitterStream and calls these adequate listener methods continuously.
	}
}
