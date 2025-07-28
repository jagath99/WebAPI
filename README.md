# WebAPI
This is a simple integration of etching data from thirdparty api and store in a sql database.
used [this](http://jsonplaceholder.typicode.com) as the thirdparty API.

# Step 01
Install visual studio and mssql if not already in your computer.
Check whether the .NET installed or modify and run intaller for .NET
start a new project with the template ASP.NET Core Web API visual studio.
Remove initially generated .cs files

# Step 02
Define Models which are the Entities going to store and quarrey. [Post](./WebAPI/Models/Post.cs) , [Comment](./WebAPI/Models/Comment.cs)
Connect Database to SQL Sever using SSMS and Creae a new data base
add Connection String to the appSetting.json file
` "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=<databaseName>;Trusted_Connection=True;" `
Then DbContext created.
install dependancies SqlServer by
`dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
this package 
  Enablecto use SQL Server as your database provider.
  Translates LINQ queries into T-SQL.
  Handles migrations for SQL Server.
install tools package 
`dotnet add package Microsoft.EntityFramworkCore.tools`
that package Required for:
  Migrations.
  Reverse engineering an existing database into EF models.
  Updating the database schema from code.
  
#Step 03
run migration commands to create tables
`dotnet ef migrations add AddPostAndComment`
`dotnet ef database update`
then implement Controllers and thirdparty connections
use Microsoft.Data.SqlClient to full control over SQL queries (no ORM).

#Step 04
Then run code by Debug button and access data by Swagger UI.
Add exceptions

#Step 05
then `dotnet build` to build 
run `dotnet run` deploy on localhost
open swagger UI Through your browser 
`http://localhost:<port>/swagger`

