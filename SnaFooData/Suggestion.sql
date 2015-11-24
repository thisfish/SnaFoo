CREATE TABLE [dbo].[Suggestion](
	[Id] [int] IDENTITY(491,1) NOT NULL,
	[SnackId] [int] NOT NULL,
	[SuggestedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Suggestions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

