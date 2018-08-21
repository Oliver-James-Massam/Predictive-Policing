CREATE TABLE [dbo].[CrimeTweets] (
    [tweet_id]          INT            IDENTITY(1,1)	NOT NULL,
    [message]           VARCHAR (160)  NOT NULL,
    [latitude]          FLOAT   NULL,
    [longitude]          FLOAT   NULL,
    [location]          VARCHAR (60)   NULL,
    [post_datetime]     DATETIME2 (7)  NOT NULL,
    [recieved_datetime] DATETIME2 (7)  NOT NULL,
    [twitter_handle]    VARCHAR (20) NOT NULL,
    [weather]           VARCHAR (MAX)  NULL,
    [mentions]          VARCHAR (MAX)  NULL,
    [tags]              VARCHAR (MAX)  NULL,
    PRIMARY KEY (tweet_id)
)

CREATE TABLE [dbo].[SVM]
(
	[sv_id] INT IDENTITY(1,1) NOT NULL,
    [support_vectors] VARCHAR(MAX) NOT NULL,
    [alphas] VARCHAR(MAX) NOT NULL,
    [weighted_sums] VARCHAR(MAX) NOT NULL,
    [label] VARCHAR(MAX) NOT NULL,
    [kernal] VARCHAR(MAX) NOT NULL,
	[tweet_id] INT NOT NULL,
	PRIMARY KEY (sv_id),
	FOREIGN KEY (tweet_id) REFERENCES CrimeTweets(tweet_id)

)

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

CREATE TABLE [dbo].[Entities]
(
	[entity_id] INT IDENTITY(1,1) NOT NULL,
	[name] VARCHAR(MAX) NOT NULL,
    [category_type] VARCHAR(MAX) NOT NULL,
    [senti_score] FLOAT NOT NULL,
    [senti_magnitude] FLOAT NOT NULL,
    [senti_salience] FLOAT NOT NULL,
    [sentiment_id] INT NOT NULL,
    PRIMARY KEY ([entity_id]),
	FOREIGN KEY (sentiment_id) REFERENCES Sentiments(sentiment_id)
);
