CREATE TABLE [Weight].[ClientMetrics]
(
	[ClientMetricId] INT NOT NULL IDENTITY(1,1), 
    [ClientId] INT NOT NULL, 
    [RecordedValue] DECIMAL(18, 2) NOT NULL, 
    [RecordedDate] DATE NOT NULL, 
    [CreatedTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    CONSTRAINT PK_ClientMetrics PRIMARY KEY ([ClientMetricId] ASC),
    CONSTRAINT FK_ClientMetrics_Clients FOREIGN KEY ([ClientId]) REFERENCES [Weight].[Clients]([ClientId])
)
