# CleanSample Backend

1- Run in package manager console this command

dotnet restore

or Resotre nuget packages in root solution

2- modify connection string located in CleanSample.Presentation appsetting.json and appsettings.Development.json to proper database and credentials

3- Run in package manager console this command after selecting target project as CleanSample.Infrastructure

Update-Database

4- Select CleanSample.Presentation as starting project and run the project, it will open swagger page
this will seed default data with credentials:
username: admin
password: admin123

# Product-portal frontend

1- Run this command in terminal after selecting the project root folder:

npm install

2- Enter credentials:
username: admin
password: admin123