CREATE TABLE [dbo].[Entities] (
    [entity_id]       INT              IDENTITY (1, 1) NOT NULL,
    [name]            VARCHAR (MAX)    NOT NULL,
    [category_type]   VARCHAR (MAX)    NULL,
    [senti_score]     DECIMAL (38, 19) NULL,
    [senti_magnitude] DECIMAL (38, 19) NULL,
    [senti_salience]  DECIMAL (38, 19) NULL,
    [sentiment_id]    INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([entity_id] ASC),
    FOREIGN KEY ([sentiment_id]) REFERENCES [dbo].[Sentiments] ([sentiment_id])
);

