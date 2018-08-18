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
)
