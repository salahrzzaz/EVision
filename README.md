# EVision
The Task divided tow project:
1-.Net Core & SQL server (code first)
  >> to run database please type this command line (dotnet ef database update ) then dotnet run
  >>the database will created automatic and dummy data
2-Angular Project
   >> to run project please run in command line (npm install) then (npm start)
   
   
   
   .Net Core Project architect in application layers as follow:
   
   1- Application.Infrastructure.Data that contains all Models & generic Repository & database extensions
   2- Application.Infrastructure.API that contains all base controllers and startup extensions
   3-Application.Data that contains our Database context and custom  Repositories
   4- Application.Web that contains our APIS
   5-Application.Test that contains unit test for APIS
