﻿CREATE TABLE [dbo].[Vote](
	[Id] [int] IDENTITY(657,1) NOT NULL,
	[SnackId] [int] NOT NULL,
	[VotedOn] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)