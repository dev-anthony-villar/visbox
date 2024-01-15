#	BoxToLocal

##	Organization

BoxToLocal
|
|-- Docs    //  Documentation 
|-- Scripts //  Build scripts
|-- Src     //  Source Code
    |
    |-- BusinessLogic   
        |
        |-- Interfaces  //  Define required methods and properties Business Serivces 
            |

        |-- Services    //  Business Services implementation 
            |   
            |-- Getters
            |
            |-- 
            |
            |-- 
    |
    |-- Common
        |
        |-- Helpers // Utilities specific to service 
            |
        |-- Utilities // General Utilities 
            |
    |
    |-- DAL
        |
        |-- DbContexts // ORM context, manages entity objects and database interactions 
            |
        |-- Repositories // Classes that provide methods to interact with the DB enitities 
            |
        |-- Entities // Models representing the tables in DB 
            |
    |
    |-- Infrastructure
        |
        |-- Configuration
            |
            |--appsettings.json
            |--secrets.json 
        |    
        |-- Logging
    |
    |-- MainApp
        |
        |-- Controllers // handles requests from view, processes them with models selects view etc interface between model and view 
        |-- Models  //  representation of the data and attributes 
        |-- Views   //  What client see's and interacts with 
    |   
    |-- ServiceIntegrations
        |
        |-- BoxService
            |
            |--BoxAuthenticator.cs // Authenticate Client 
            |
            |--BoxAPIClient.cs      // Interacts with Box.com API
            |
            |--BoxAPIProcessor.cs   // prep data for insert / import
            |
    |
|-- Resources // data diles etc 
|-- Tests   //  Tests



##  Box General Notes 

    *   when using folder search get folder by name then retrieve folderID and use that to retrieve others 

