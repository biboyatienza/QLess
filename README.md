# CogSoft Exam Advisory 3.18.22
## Technologies used:
- VSCode as Editor
- Flixs.vs-code-http-server-and-html-preview use as api testing
- .Net Core 6
- webapi, api
- c#
- RegEx for format validation for Sr. Citizen ID and PWD ID format

## Instructions running the project:
- cd qless
- code .
- Install REST Client for Visual Studio Code by Huachao Mao
- Ctrl + ` (open vscode terminal)
- dotnet restore
- dotnet build
- dotnet run
- Please refer to "test.http" file included on the root of this project for testing scenario



## Logs:
* 19.March.2022 04:30 - Started reading the machine problem.
* 04:45 - Analyzing my approach... if all requirements are posible to run on a console app only.  
* 05:05 - desided to go with API using this guide -> https://docs.microsoft.com/en-us/learn/modules/build-web-api-aspnet-core/
* 05:06 - Playing KMKZ Acoustic on the background.
* 05:10 - Open VSCode and create Q-LESS Project
  ```
        $ dotnet new webapi -o qless
        $ cd qless
        $ dotnet restore
        $ dotnet build
        $ dotnet run
  ```      
* 05:15 - create test.http, testing the default api using REST Client, REST Client for Visual Studio Code by Huachao Mao
* 06:43 - Done with base webapi, now applying request validation schema
* 07:37 - Got bare working Section A and B
* 07:54 - Playing https://regexr.com/
      *  Sr. Citizen ID:
        ```        
        ^([a-zA-Z0-9]{2})\-{1}([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})  
        ```
      *  PWD ID:
        ```
        ^([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})\-{1}([a-zA-Z0-9]{4})
        ```
* 08:29 - Applied Regex validation on both Senior Citizen Number and PWD ID
* 08:30 - Planning fare rate strategy for Section A and background
* 08:58 - BREAK BreakFast!
* 10:00 - Resume
* 11:26 - Tested OK for Section A and B, all are working.
* 11:27 - Going to Section C
* 12:50 - Finished Section A and B test.http documentation
* 13:05 - Done uploading to shared gdrive for Section A & Section B
* 13:06 - Continue on Section c
* 13:30 - BREAK, lunch
* 16:05 - Trying Section C
* 17:24 - Done coding for Section C, now testing
* 17.51 - Done testing Section C, all are working, please se test,http for may test samples
* 17:54 - Uploaded to shared gdrive







