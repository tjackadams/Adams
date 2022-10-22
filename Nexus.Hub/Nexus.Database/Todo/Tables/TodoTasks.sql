CREATE TABLE [Todo].[TodoTasks]
(
	[TodoTaskId] INT NOT NULL IDENTITY(1,1),
    [TodoId] INT NOT NULL,
    [Title] NVARCHAR(2000) NOT NULL, 
    CONSTRAINT PK_TodoTasks PRIMARY KEY ([TodoTaskId] ASC),
    CONSTRAINT FK_TodoTasks_Todo FOREIGN KEY ([TodoId]) REFERENCES [Todo].[Todos]([TodoId])
)
