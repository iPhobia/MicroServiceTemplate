# MicroServiceTemplate

Internship task which goal was to create template for some microservice.
Here WCF web-service is hosted localy in IIS handles requests from client. 
More specifically, request to exact endpoint invokes WCF Operation. This Operation takes data from request and invokes stored procedure in database via Dapper/ADO.NET. Stored procedure adds record to a table in database. Records in this table are processed by WinNT Service, which works in a "Queue" manner. Once in a given interval it fetches newly added records and processes them by some logic and updates according to the result of processing. With the help of log4net requests to WCF are logged centralized to database and logfiles.
