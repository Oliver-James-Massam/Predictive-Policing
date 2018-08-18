CREATE TABLE [dbo].[CrimeTweets] (
    [tweet_id]          INT            IDENTITY(1,1)	NOT NULL,
    [message]           VARCHAR (160)  NOT NULL,
    [latitude]          FLOAT   NULL,
    [logitude]          FLOAT   NULL,
    [location]          VARCHAR (60)   NULL,
    [post_datetime]     DATETIME2 (7)  NOT NULL,
    [recieved_datetime] DATETIME2 (7)  NOT NULL,
    [twitter_handle]    VARCHAR (20) NOT NULL,
    [weather]           VARCHAR (MAX)  NULL,
    [mentions]          VARCHAR (MAX)  NULL,
    [tags]              VARCHAR (MAX)  NULL,
    PRIMARY KEY (tweet_id)
);

