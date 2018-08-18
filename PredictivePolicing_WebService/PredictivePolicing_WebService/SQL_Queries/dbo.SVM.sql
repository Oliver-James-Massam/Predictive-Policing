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
