SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suggestion](
	[Id] [int] IDENTITY(491,1) NOT NULL,
	[SnackId] [int] NOT NULL,
	[SuggestedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Suggestions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vote](
	[Id] [int] IDENTITY(657,1) NOT NULL,
	[SnackId] [int] NOT NULL,
	[VotedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
GO
-- BCPArgs:10:[dbo].[Suggestion] in "c:\SQLAzureMW\BCPData\dbo.Suggestion.dat" -E -n -C RAW -b 1000 -a 4096
GO
-- BCPArgs:37:[dbo].[Vote] in "c:\SQLAzureMW\BCPData\dbo.Vote.dat" -E -n -C RAW -b 1000 -a 4096
GO

