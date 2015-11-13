Features
===================== 

* List of all your service components
* Logging of incidents (in-progress)
* Full Pingdom Support
* Deployable in hosted environment


Requirements
======================

* IIS with support for ASP.NET 4.5 or above
* Microsoft SQL Server
* Full-Trust Environment (this can be reduced using methods listed below)


Installation
============================

1. Download latest build from Github releases.  
2. Extract all files.  
3. Edit app.config located in the root.  

	<?xml version="1.0"?>
		<appSettings>
			<!-- Set config to True when all values are entered, if set to false you will be presented with a configuration page -->
  			<add key="config" value="True" /> 
  			<!-- [Optional] Pingdom Email Address for main account -->
  			<add key="pingdomEmail" value="" />
  			<!-- [Optional] Pingdom Password for API Access -->
  			<add key="pingdomPassword" value="" />
  			<!-- [Optional] Pingdom App Key for API Access -->
  			<add key="pingdomAppKey" value="" />
  			<!-- [REQUIRED] MS SQL Database address, eg. IPADDRESS,1433 [PORT IS OPTIONAL]-->
  			<add key="databaseAddress" value="" />
  			<!-- [REQUIRED] Database Name, note on launch Public Status will create all the required tables -->
  			<add key="databaseName" value="" />
  			<!-- [REQUIRED] Database Username with WRITE permissions-->
  			<add key="databaseUserName" value="" />
  			<!-- [REQUIRED] Database Password with WRITE permissions-->
  			<add key="databasePassword" value="" />
		</appSettings>


4. Copy all files to your IIS server and launch it in your web browser. Browsing to the root url, eg. http://localhost/ will automatically create all the required SQL tables.
5. You are all set to start using Public Status.


Use without Hangfire (Medium Trust)
=======================================

If your host requires your site to use medium trust, you cannot use Public Status out of the box. You must open the development branch and remove all Hangfire References. After removeing Hangfire setup a cron task requesting the following urls at 1 minute increments.

/_cron/cronUpdateChecks.ashx

