--Common Tasks...
Select * From Files
Select * From Images
Select * From WebPages

Select * From HyperLinks

--Troubleshooting...
Select Top 1000 * From Exceptions Order by Created DESC
Select * From DisallowedAbsoluteUris

--Crawl state (where the crawl is currently at, or after pressing CTRL-C)...
Select * From CrawlRequests
Select * From Discoveries

--Reset the Crawler state...
--Delete From Discoveries
--Delete From CrawlRequests

--Check Plugin Configs...
Select * From cfg.CrawlActions
Select * From cfg.CrawlRules

--Check tables corresponding to 'ClassifyAbsoluteUris'
Select * From [Domains]
Select * From Hosts
