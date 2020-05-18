RESTORE DATABASE [arachnode.net] FROM  DISK = N'C:\Databases\arachnode.net.bak' WITH  FILE = 1,
MOVE N'arachnode.net_dat' TO N'C:\Databases\arachnode.net.mdf',
MOVE N'arachnode.net_CrawlRequests' TO N'C:\Databases\arachnode.net_CrawlRequests.ndf',
MOVE N'arachnode.net_EmailAddresses' TO N'C:\Databases\arachnode.net_EmailAddresses.ndf',
MOVE N'arachnode.net_Exceptions' TO N'C:\Databases\arachnode.net_Exceptions.ndf',
MOVE N'arachnode.net_Files' TO N'C:\Databases\arachnode.net_Files.ndf',
MOVE N'arachnode.net_HyperLinks' TO N'C:\Databases\arachnode.net_HyperLinks.ndf',
MOVE N'arachnode.net_WebPages' TO N'C:\Databases\arachnode.net_WebPages.ndf',
MOVE N'arachnode.net_Files_MetaData' TO N'C:\Databases\arachnode.net_Files_MetaData.ndf',
MOVE N'arachnode.net_WebPages_MetaData' TO N'C:\Databases\arachnode.net_WebPages_MetaData.ndf',
MOVE N'arachnode.net_Reporting' TO N'C:\Databases\arachnode.net_Reporting.ndf',
MOVE N'arachnode.net_Cache' TO N'C:\Databases\arachnode.net_Cache.ndf',
MOVE N'arachnode.net_DisallowedAbsoluteUris' TO N'C:\Databases\arachnode.net_DisallowedAbsoluteUris.ndf',
MOVE N'arachnode.net_Images' TO N'C:\Databases\arachnode.net_Images.ndf',
MOVE N'arachnode.net_Images_MetaData' TO N'C:\Databases\arachnode.net_Images_MetaData.ndf',
MOVE N'ftrow_arachnode.net_ftc_Files' TO N'C:\Databases\arachnode.net_ftc_Files.ndf',
MOVE N'ftrow_arachnode.net_ftc_WebPages' TO N'C:\Databases\arachnode.net_ftc_WebPages.ndf',
MOVE N'ftrow_arachnode.net_ftc_WebPages_MetaData' TO N'C:\Databases\arachnode.net_ftc_WebPages_MetaData.ndf',
MOVE N'ftrow_arachnode.net_ftc_Files_MetaData' TO N'C:\Databases\arachnode.net_ftc_Files_MetaData.ndf',
MOVE N'ftrow_arachnode.net_ftc_Images' TO N'C:\Databases\arachnode.net_ftc_Images.ndf',
MOVE N'ftrow_arachnode.net_ftc_Images_MetaData' TO N'C:\Databases\arachnode.net_ftc_Images_MetaData.ndf',
MOVE N'arachnode.net_Documents' TO N'C:\Databases\arachnode.net_Documents.ndf',
MOVE N'arachnode.net_Configuration' TO N'C:\Databases\arachnode.net_Configuration.ndf',
MOVE N'arachnode.net_log' TO N'C:\Databases\arachnode.net.ldf', NOUNLOAD, REPLACE, STATS = 10
GO

USE [arachnode.net]

EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE]
GO