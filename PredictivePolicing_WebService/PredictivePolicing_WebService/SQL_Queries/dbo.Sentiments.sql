CREATE TABLE [dbo].[Sentiments]
(
	[sentiment_id] INT IDENTITY(1,1) NOT NULL,
	[sentiment_total] FLOAT NOT NULL, 
    [category_primary] VARCHAR(MAX) NOT NULL, 
    [key_phrases] VARCHAR(MAX) NOT NULL, 
    [tweet_id] INT NOT NULL, 
    PRIMARY KEY (sentiment_id),
	FOREIGN KEY (tweet_id) REFERENCES CrimeTweets(tweet_id)
)
